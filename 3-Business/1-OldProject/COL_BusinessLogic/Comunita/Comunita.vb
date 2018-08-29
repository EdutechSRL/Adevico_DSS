Imports COL_DataLayer
'Imports COL_BusinessLogic_v2.SMScomponent
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_persona.CL_Esterno
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comol.Manager


Namespace Comunita
    Public Class COL_Comunita
        Inherits ObjectBase
        Implements IEquatable(Of COL_Comunita)


#Region "Private Property"
        Protected n_CMNT_id As Integer
        Protected n_CMNT_idpadre As Integer
        Protected n_CMNT_nome As String
        Protected n_CMNT_PRSN_ID As Integer
        Protected n_CMNT_datacreazione As DateTime
        Protected n_CMNT_datacessazione As DateTime
        Protected n_CMNT_dataInizioIscrizione As DateTime
        Protected n_CMNT_dataFineIscrizione As DateTime
        Protected n_CMNT_Anno As Integer
        Protected n_CMNT_statuto As String
        Protected n_CMNT_mdcm As New COL_Modello_Comunita
        Protected n_CMNT_tpcm As New COL_Tipo_Comunita
        Protected n_CMNT_orgn As New COL_Organizzazione
        Protected n_CMNT_ischiusa As Boolean
        Protected n_CMNT_Bloccata As Integer
        Protected n_CMNT_Archiviata As Integer
        Protected n_CMNT_livello As Integer
        Protected n_CMNT_MaxIscritti As Integer

        Protected n_CMNT_MaxIscrittiOverList As Integer
        Protected n_CMNT_CanSubscribe As Integer
        Protected n_CMNT_CanUnsubscribe As Integer
        Protected n_CMNT_AccessoLibero As Integer
        Protected n_CMNT_AccessoCopisteria As Integer
        Protected n_Errore As Errori_Db
        Protected _Creatore As COL_Persona
        Protected _Responsabile As COL_Persona


        Protected n_CMNT_code As String
        Protected n_CMNT_description As String
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_CMNT_id
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_id = Value
            End Set
        End Property
        Public Property IdPadre() As Integer
            Get
                IdPadre = n_CMNT_idpadre
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_idpadre = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_CMNT_nome
            End Get
            Set(ByVal Value As String)
                n_CMNT_nome = Value
            End Set
        End Property
        Public Property CreatoreID() As Integer
            Get
                CreatoreID = n_CMNT_PRSN_ID
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_PRSN_ID = Value
            End Set
        End Property
        Public Property DataCreazione() As DateTime
            Get
                DataCreazione = n_CMNT_datacreazione
            End Get
            Set(ByVal Value As DateTime)
                n_CMNT_datacreazione = Value
            End Set
        End Property
        Public Property DataCessazione() As DateTime
            Get
                DataCessazione = n_CMNT_datacessazione
            End Get
            Set(ByVal Value As DateTime)
                n_CMNT_datacessazione = Value
            End Set
        End Property
        Public Property DataInizioIscrizione() As DateTime
            Get
                DataInizioIscrizione = n_CMNT_dataInizioIscrizione
            End Get
            Set(ByVal Value As DateTime)
                n_CMNT_dataInizioIscrizione = Value
            End Set
        End Property
        Public Property DataFineIscrizione() As DateTime
            Get
                DataFineIscrizione = n_CMNT_dataFineIscrizione
            End Get
            Set(ByVal Value As DateTime)
                n_CMNT_dataFineIscrizione = Value
            End Set
        End Property
        Public Property AnnoCreazione() As Integer
            Get
                AnnoCreazione = n_CMNT_Anno
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_Anno = Value
            End Set
        End Property
        Public Property Statuto() As String
            Get
                Statuto = n_CMNT_statuto
            End Get
            Set(ByVal Value As String)
                n_CMNT_statuto = Value
            End Set
        End Property
        Public Property ModelloComunita() As COL_Modello_Comunita
            Get
                ModelloComunita = n_CMNT_mdcm
            End Get
            Set(ByVal Value As COL_Modello_Comunita)
                n_CMNT_mdcm = Value
            End Set
        End Property
        Public Property TipoComunita() As COL_Tipo_Comunita
            Get
                TipoComunita = n_CMNT_tpcm
            End Get
            Set(ByVal Value As COL_Tipo_Comunita)
                n_CMNT_tpcm = Value
            End Set
        End Property
        Public Property Organizzazione() As COL_Organizzazione
            Get
                Organizzazione = n_CMNT_orgn
            End Get
            Set(ByVal Value As COL_Organizzazione)
                n_CMNT_orgn = Value
            End Set
        End Property
        Public Property IsChiusa() As Boolean
            Get
                IsChiusa = n_CMNT_ischiusa
            End Get
            Set(ByVal Value As Boolean)
                n_CMNT_ischiusa = Value
            End Set
        End Property
        Public Property Livello() As Integer
            Get
                Livello = n_CMNT_livello
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_livello = Value
            End Set
        End Property
        Public Property MaxIscritti() As Integer
            Get
                MaxIscritti = n_CMNT_MaxIscritti
            End Get
            Set(ByVal Value As Integer)
                n_CMNT_MaxIscritti = Value
            End Set
        End Property

        Public Property CanSubscribe() As Boolean
            Get
                CanSubscribe = (n_CMNT_CanSubscribe = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_CMNT_CanSubscribe = 1
                Else
                    n_CMNT_CanSubscribe = 0
                End If
            End Set
        End Property
        Public Property CanUnsubscribe() As Boolean
            Get
                CanUnsubscribe = (n_CMNT_CanUnsubscribe = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_CMNT_CanUnsubscribe = 1
                Else
                    n_CMNT_CanUnsubscribe = 0
                End If
            End Set
        End Property
        Public Property OverMaxIscritti() As Integer
            Get
                OverMaxIscritti = n_CMNT_MaxIscrittiOverList
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Value = 0
                End If
                n_CMNT_MaxIscrittiOverList = Value
            End Set
        End Property

        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_Errore
            End Get
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

        Public Property HasAccessoLibero() As Boolean
            Get
                HasAccessoLibero = n_CMNT_AccessoLibero
            End Get
            Set(ByVal Value As Boolean)
                n_CMNT_AccessoLibero = Value
            End Set
        End Property
        Public Property HasAccessoCopisteria() As Boolean
            Get
                HasAccessoCopisteria = n_CMNT_AccessoCopisteria
            End Get
            Set(ByVal Value As Boolean)
                n_CMNT_AccessoCopisteria = Value
            End Set
        End Property


        Public Property Code() As String
            Get
                Return n_CMNT_code
            End Get
            Set(ByVal Value As String)
                n_CMNT_code = Value
            End Set
        End Property
        Public Property Description() As String
            Get
                If String.IsNullOrWhiteSpace(n_CMNT_description) Then
                    n_CMNT_description = n_CMNT_statuto
                End If

                Return n_CMNT_description
            End Get
            Set(ByVal Value As String)
                n_CMNT_description = Value
            End Set
        End Property

#End Region

#Region "Enum Filtri"
        Public Enum FiltroCampoOrdine
            nessuno = 0
            anagrafica = 1
            dataNascita = 2
            tipoRuolo = 3
            login = 4
            tipoPersona = 5
            dataIscrizione = 6
            nome = 7
            cognome = 8
        End Enum
#End Region

#Region "Metodi New"
        Public Sub New()
            n_CMNT_id = 0
            n_CMNT_idpadre = 0
            n_CMNT_mdcm.Id = 0
            n_CMNT_tpcm.ID = 0
            n_CMNT_orgn.Id = 0
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_CMNT_PRSN_ID = 0
            Me.n_CMNT_ischiusa = False
            Me.n_CMNT_MaxIscritti = 0
            Me.n_CMNT_CanSubscribe = 1
            Me.n_CMNT_CanUnsubscribe = 1
            Me.n_CMNT_MaxIscrittiOverList = 0
            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_Errore = Errori_Db.None
        End Sub
        Public Sub New(ByVal ComunitaID As Integer)
            n_CMNT_id = ComunitaID
            n_CMNT_idpadre = 0
            n_CMNT_mdcm.Id = 0
            n_CMNT_tpcm.ID = 0
            n_CMNT_orgn.Id = 0
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_CMNT_PRSN_ID = 0
            Me.n_CMNT_ischiusa = False
            Me.n_CMNT_MaxIscritti = 0
            Me.n_CMNT_CanSubscribe = 1
            Me.n_CMNT_CanUnsubscribe = 1
            Me.n_CMNT_MaxIscrittiOverList = 0
            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_Errore = Errori_Db.None
        End Sub
        Public Sub New(ByVal ComunitaID As Integer, ByVal TipoComunitaID As Integer)
            n_CMNT_id = ComunitaID
            n_CMNT_idpadre = 0
            n_CMNT_mdcm.Id = 0
            n_CMNT_tpcm.ID = 0
            n_CMNT_orgn.Id = 0
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_CMNT_PRSN_ID = 0
            Me.n_CMNT_ischiusa = False
            Me.n_CMNT_MaxIscritti = 0
            Me.n_CMNT_CanSubscribe = 1
            Me.n_CMNT_CanUnsubscribe = 1
            Me.n_CMNT_MaxIscrittiOverList = 0
            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_Errore = Errori_Db.None
        End Sub
        Public Sub New(ByVal ComunitaID As Integer, ByVal nome As String, ByVal PadreID As Integer)
            n_CMNT_id = ComunitaID
            Me.n_CMNT_nome = nome
            n_CMNT_idpadre = PadreID
            n_CMNT_mdcm.Id = 0
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_CMNT_PRSN_ID = 0
            Me.n_CMNT_ischiusa = False
            Me.n_CMNT_MaxIscritti = 0
            Me.n_CMNT_CanSubscribe = 1
            Me.n_CMNT_CanUnsubscribe = 1
            Me.n_CMNT_MaxIscrittiOverList = 0
            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_Errore = Errori_Db.None
        End Sub
        Public Sub New(ByVal ComunitaID As Integer, ByVal nome As String, ByVal Organizzazione As COL_Organizzazione, ByVal TipoComunita As COL_Tipo_Comunita)
            n_CMNT_id = ComunitaID
            Me.n_CMNT_nome = nome
            Me.n_CMNT_orgn = Organizzazione
            Me.n_CMNT_tpcm = TipoComunita
            n_CMNT_idpadre = 0
            n_CMNT_mdcm.Id = 0
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_CMNT_PRSN_ID = 0
            Me.n_CMNT_ischiusa = False
            Me.n_CMNT_MaxIscritti = 0
            Me.n_CMNT_CanSubscribe = 1
            Me.n_CMNT_CanUnsubscribe = 1
            Me.n_CMNT_MaxIscrittiOverList = 0
            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_Errore = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
        Public Sub Estrai()

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_EstraiComunita_b"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacreazione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacessazione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_dataInizioIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_dataFineIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_statuto", "", ParameterDirection.Output, DbType.String, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_mdcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_orgn_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", "", ParameterDirection.Output, DbType.Boolean, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_orgn_ragSoc", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_PRSN_ID", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_livello", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanSubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanUnsubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscrittiOverList", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_Archiviata", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_Bloccata", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_code", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_description", "", ParameterDirection.Output, DbType.String, , 1073741823)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_CMNT_idpadre = oRequest.GetValueFromParameter(2)
                Me.n_CMNT_nome = oRequest.GetValueFromParameter(3)
                Me.n_CMNT_datacreazione = oRequest.GetValueFromParameter(4)
                If oRequest.GetValueFromParameter(5) <> "" Then
                    Me.n_CMNT_datacessazione = oRequest.GetValueFromParameter(5)
                End If
                If oRequest.GetValueFromParameter(6) <> "" Then
                    Me.n_CMNT_dataInizioIscrizione = oRequest.GetValueFromParameter(6)
                End If
                If oRequest.GetValueFromParameter(7) <> "" Then
                    Me.n_CMNT_dataFineIscrizione = oRequest.GetValueFromParameter(7)
                End If

                Me.n_CMNT_statuto = oRequest.GetValueFromParameter(8)
                Me.n_CMNT_mdcm.Id = oRequest.GetValueFromParameter(9)
                Me.n_CMNT_tpcm.ID = oRequest.GetValueFromParameter(10)

                Me.n_CMNT_orgn.Id = oRequest.GetValueFromParameter(11)
                Me.n_CMNT_ischiusa = oRequest.GetValueFromParameter(12)
                Me.n_CMNT_tpcm.Icona = oRequest.GetValueFromParameter(13)
                Me.n_CMNT_tpcm.Descrizione = oRequest.GetValueFromParameter(14)
                Me.n_CMNT_orgn.RagioneSociale = oRequest.GetValueFromParameter(15)
                Me.n_CMNT_PRSN_ID = oRequest.GetValueFromParameter(16)
                Me.n_CMNT_livello = oRequest.GetValueFromParameter(17)
                Me.n_CMNT_MaxIscritti = oRequest.GetValueFromParameter(18)
                Me.n_CMNT_CanSubscribe = oRequest.GetValueFromParameter(19)
                Me.n_CMNT_CanUnsubscribe = oRequest.GetValueFromParameter(20)
                Me.n_CMNT_MaxIscrittiOverList = oRequest.GetValueFromParameter(21)
                Me.n_CMNT_Archiviata = oRequest.GetValueFromParameter(22)
                Me.n_CMNT_Bloccata = oRequest.GetValueFromParameter(23)
                Me.n_CMNT_AccessoLibero = oRequest.GetValueFromParameter(24)
                Me.n_CMNT_AccessoCopisteria = oRequest.GetValueFromParameter(25)

                Me.n_CMNT_code = oRequest.GetValueFromParameter(26)
                Me.n_CMNT_description = oRequest.GetValueFromParameter(27)

                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub EstraiByLingua(ByVal LinguaID As Integer)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_EstraiByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacreazione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacessazione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_dataInizioIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_dataFineIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_statuto", "", ParameterDirection.Output, DbType.String, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_mdcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_orgn_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", "", ParameterDirection.Output, DbType.Boolean, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_orgn_ragSoc", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_PRSN_ID", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_livello", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanSubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanUnsubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscrittiOverList", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32, False)
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

                Me.n_CMNT_idpadre = oRequest.GetValueFromParameter(2)
                Me.n_CMNT_nome = oRequest.GetValueFromParameter(3)
                Me.n_CMNT_datacreazione = oRequest.GetValueFromParameter(4)
                If oRequest.GetValueFromParameter(5) <> "" Then
                    Me.n_CMNT_datacessazione = oRequest.GetValueFromParameter(5)
                End If
                If oRequest.GetValueFromParameter(6) <> "" Then
                    Me.n_CMNT_dataInizioIscrizione = oRequest.GetValueFromParameter(6)
                End If
                If oRequest.GetValueFromParameter(7) <> "" Then
                    Me.n_CMNT_dataFineIscrizione = oRequest.GetValueFromParameter(7)
                End If

                Me.n_CMNT_statuto = oRequest.GetValueFromParameter(8)
                Me.n_CMNT_mdcm.Id = oRequest.GetValueFromParameter(9)
                Me.n_CMNT_tpcm.ID = oRequest.GetValueFromParameter(10)

                Me.n_CMNT_orgn.Id = oRequest.GetValueFromParameter(11)
                Me.n_CMNT_ischiusa = oRequest.GetValueFromParameter(12)
                Me.n_CMNT_tpcm.Icona = oRequest.GetValueFromParameter(13)
                Me.n_CMNT_tpcm.Descrizione = oRequest.GetValueFromParameter(14)
                Me.n_CMNT_orgn.RagioneSociale = oRequest.GetValueFromParameter(15)
                Me.n_CMNT_PRSN_ID = oRequest.GetValueFromParameter(16)
                Me.n_CMNT_livello = oRequest.GetValueFromParameter(17)
                Me.n_CMNT_MaxIscritti = oRequest.GetValueFromParameter(18)

                Me.n_CMNT_CanSubscribe = oRequest.GetValueFromParameter(19)
                Me.n_CMNT_CanUnsubscribe = oRequest.GetValueFromParameter(20)
                Me.n_CMNT_MaxIscrittiOverList = oRequest.GetValueFromParameter(21)
                Me.n_CMNT_Archiviata = oRequest.GetValueFromParameter(23)
                Me.n_CMNT_Bloccata = oRequest.GetValueFromParameter(24)
                Me.n_CMNT_AccessoLibero = oRequest.GetValueFromParameter(25)
                Me.n_CMNT_AccessoCopisteria = oRequest.GetValueFromParameter(26)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Overridable Function Aggiungi(ByVal IdCreatore As Integer, ByVal SmsRicezione As Integer, ByVal Iscrivi As Boolean) As Integer
            Return AggiungiComunita(IdCreatore, SmsRicezione, Iscrivi)
        End Function

        Public Overridable Sub Modifica()
            Me.ModificaComunita()
        End Sub

        Protected Function AggiungiComunita(ByVal IdCreatore As Integer, ByVal SmsRicezione As Integer, ByVal Iscrivi As Boolean) As Integer
            'se riesce ad aggiungere setta l'id dell'oggetto comunità a quello appena inserito nel db
            ' altrimenti gli mette -1
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Me.n_CMNT_datacreazione = Now 'data completa (true predefinito)
            Dim dataCessazione, DataInizioIscr, DataFineIscr As String
            If Equals(Me.n_CMNT_datacessazione, New Date) Then '"12:00AM" 
                dataCessazione = ""
            Else
                dataCessazione = DateToString(Me.n_CMNT_datacessazione)
            End If
            If Equals(Me.n_CMNT_dataFineIscrizione, New Date) Then  '"12:00AM" 
                DataFineIscr = ""
            Else
                DataFineIscr = DateToString(Me.n_CMNT_dataFineIscrizione)
            End If
            If Equals(Me.n_CMNT_dataInizioIscrizione, New Date) Then  '"12:00AM" 
                DataInizioIscr = ""
            Else
                DataInizioIscr = DateToString(Me.n_CMNT_dataInizioIscrizione)
            End If

            With oRequest
                .Command = "sp_Comunita_Aggiungi_b"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, DbType.String, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", n_CMNT_idpadre, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", n_CMNT_nome.Trim, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacreazione", DateToString(n_CMNT_datacreazione), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacessazione", dataCessazione, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datainizioiscrizione", DataInizioIscr, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datafineiscrizione", DataFineIscr, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_statuto", n_CMNT_statuto, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_TPCM_id", n_CMNT_tpcm.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MDCM_id", n_CMNT_mdcm.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ORGN_id", n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", n_CMNT_ischiusa, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Creatore", IdCreatore, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SMS_Ricezione", SmsRicezione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_PRSN_ID", IdCreatore, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_livello", n_CMNT_livello, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If Iscrivi Then
                    oParam = objAccesso.GetParameter("@Iscrivi", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Iscrivi", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", Me.n_CMNT_MaxIscritti, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@CMNT_CanSubscribe", Me.n_CMNT_CanSubscribe, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanUnsubscribe", Me.n_CMNT_CanUnsubscribe, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscrittiOverList", Me.n_CMNT_MaxIscrittiOverList, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_Archiviata", Me.n_CMNT_Archiviata, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_Bloccata", Me.n_CMNT_Bloccata, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", Me.n_CMNT_AccessoLibero, ParameterDirection.Input, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", Me.n_CMNT_AccessoCopisteria, ParameterDirection.Input, DbType.Int32, False)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_code", n_CMNT_code, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_description", n_CMNT_description, ParameterDirection.Input, DbType.String, , 1073741823)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                'assegna all'oggetto l'id della comunità appena creata

                Me.n_CMNT_id = oRequest.GetValueFromParameter(1)
                Me.n_CMNT_tpcm.Icona = oRequest.GetValueFromParameter(2)
                Me.n_CMNT_PRSN_ID = IdCreatore
                'associa i permessi 
                'TUTTO IN AUTOMATICO:MARMOTTE è UN CASINO ALTRIMENTI !!!!
                ' Me.AssociaPermessiByRuoloAndOrganizzazione()

            Catch
                ' se nn è avvenuto l'inserimento setta a -1 l'id della comunità
                Me.n_CMNT_id = -1
                n_Errore = Errori_Db.DBInsert
            End Try
        End Function 'inserisce nuova comunità e gli associa chi la crea come responsabile
        Protected Sub ClonaComunita(ByVal CloneID As Integer, ByVal IdCreatore As Integer, ByVal SmsRicezione As Integer, ByVal Iscrivi As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Me.n_CMNT_datacreazione = Now 'data completa (true predefinito)
            Dim dataCessazione, DataInizioIscr, DataFineIscr As String
            If Equals(Me.n_CMNT_datacessazione, New Date) Then '"12:00AM" 
                dataCessazione = ""
            Else
                dataCessazione = DateToString(Me.n_CMNT_datacessazione)
            End If
            If Equals(Me.n_CMNT_dataFineIscrizione, New Date) Then  '"12:00AM" 
                DataFineIscr = ""
            Else
                DataFineIscr = DateToString(Me.n_CMNT_dataFineIscrizione)
            End If
            If Equals(Me.n_CMNT_dataInizioIscrizione, New Date) Then  '"12:00AM" 
                DataInizioIscr = ""
            Else
                DataInizioIscr = DateToString(Me.n_CMNT_dataInizioIscrizione)
            End If

            With oRequest
                .Command = "sp_Comunita_Clona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_idpadre", n_CMNT_idpadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_nome", n_CMNT_nome.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_datacreazione", DateToString(n_CMNT_datacreazione), ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_datacessazione", dataCessazione, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_datainizioiscrizione", DataInizioIscr, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_datafineiscrizione", DataFineIscr, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_statuto", n_CMNT_statuto, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_TPCM_id", n_CMNT_tpcm.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_MDCM_id", n_CMNT_mdcm.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ORGN_id", n_CMNT_orgn.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                If n_CMNT_ischiusa Then
                    oParam = objAccesso.GetAdvancedParameter("@CMNT_ischiusa", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@CMNT_ischiusa", 0, ParameterDirection.Input, SqlDbType.Int)
                End If

                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Creatore", IdCreatore, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SMS_Ricezione", SmsRicezione, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_PRSN_ID", IdCreatore, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_livello", n_CMNT_livello, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                If Iscrivi Then
                    oParam = objAccesso.GetAdvancedParameter("@Iscrivi", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@Iscrivi", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_MaxIscritti", Me.n_CMNT_MaxIscritti, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetAdvancedParameter("@CMNT_CanSubscribe", Me.n_CMNT_CanSubscribe, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_CanUnsubscribe", Me.n_CMNT_CanUnsubscribe, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_MaxIscrittiOverList", Me.n_CMNT_MaxIscrittiOverList, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_Archiviata", Me.n_CMNT_Archiviata, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Bloccata", Me.n_CMNT_Bloccata, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_AccessoLibero", Me.n_CMNT_AccessoLibero, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_AccessoCopisteria", Me.n_CMNT_AccessoCopisteria, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CloneID", CloneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CMNT_id = oRequest.GetValueFromParameter(1)
                Me.n_CMNT_tpcm.Icona = oRequest.GetValueFromParameter(2)
                Me.n_CMNT_PRSN_ID = IdCreatore
                n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_CMNT_id = -1
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub      'inserisce nuova comunità e gli associa chi la crea come responsabile

        Protected Sub ModificaComunita()
            'esegue la modifica del record corrispondente all'id dell'oggetto
            'non ritorna niente
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim dataCessazione, DataInizioIscr, DataFineIscr As String

            If Equals(Me.n_CMNT_datacessazione, New Date) Then '"12:00AM" 
                dataCessazione = Nothing
            Else
                dataCessazione = DateToString(Me.n_CMNT_datacessazione)
            End If
            If Equals(Me.n_CMNT_dataFineIscrizione, New Date) Then  '"12:00AM" 
                DataFineIscr = Nothing
            Else
                DataFineIscr = DateToString(Me.n_CMNT_dataFineIscrizione)
            End If
            If Equals(Me.n_CMNT_dataInizioIscrizione, New Date) Then  '"12:00AM" 
                DataInizioIscr = Nothing
            Else
                DataInizioIscr = DateToString(Me.n_CMNT_dataInizioIscrizione)
            End If

            With oRequest
                .Command = "sp_Comunita_Modifica_b"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", n_CMNT_idpadre, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", n_CMNT_nome.Trim, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacessazione", dataCessazione, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datainizioiscrizione", DataInizioIscr, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datafineiscrizione", DataFineIscr, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_statuto", n_CMNT_statuto, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_TPCM_id", n_CMNT_tpcm.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MDCM_id", n_CMNT_mdcm.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ORGN_id", n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", n_CMNT_ischiusa, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_livello", n_CMNT_livello, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", Me.n_CMNT_MaxIscritti, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_CanSubscribe", Me.n_CMNT_CanSubscribe, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_CanUnsubscribe", Me.n_CMNT_CanUnsubscribe, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MaxIscrittiOverList", Me.n_CMNT_MaxIscrittiOverList, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_Archiviata", Me.n_CMNT_Archiviata, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_Bloccata", Me.n_CMNT_Bloccata, ParameterDirection.Input, DbType.Int32)

                oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", Me.n_CMNT_AccessoLibero, ParameterDirection.Input, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", Me.n_CMNT_AccessoCopisteria, ParameterDirection.Input, DbType.Int32, False)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@CMNT_code", n_CMNT_code, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_description", n_CMNT_description, ParameterDirection.Input, DbType.String, , 1073741823)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Sub AssociaPadre(ByVal IdPadre As Integer) 'aggiunge un nuovo padre alla comunità 

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AssociaPadre"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", IdPadre, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        Public Sub AssociaPadri(ByVal Condition As String) 'aggiunge alla comunità i padri passati nella lista

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AssociaPadri"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Condition", Condition, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function HasSottoComunita() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasSottoComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                If oRequest.GetValueFromParameter(2) > 0 Then
                    iResponse = True
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
#End Region

        Public Function RegistraAccesso(ByVal idComunita As Integer, ByVal PRSN_ID As Integer, ByVal DatabaseID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_RegistraAccesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", idComunita, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", DatabaseID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return 0
            Catch ax As Exception
                Return -1
            End Try
        End Function
        Public Sub RegistraAccesso(ByVal PRSN_ID As Integer, ByVal DatabaseID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_RegistraAccesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", DatabaseID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub GetFromAccessoNonAutenticato(ByVal UrlClear As String, ByVal UrlEncoded As String)
            Dim RandomCode, testo As String
            Dim oCrypt As New COL_Encrypter

            Try
                RandomCode = UrlClear.Remove(0, 6)
                testo = COL_Encrypter.GenerateHashDigest(RandomCode & "Libero", COL_Encrypter.HashMethod.SHA1)
                If testo = UrlEncoded Then
                    Dim oRequest As New COL_Request
                    Dim oParam As New COL_Request.Parameter
                    Dim objAccesso As New COL_DataAccess
                    With oRequest
                        .Command = "sp_Comunita_EstraiComunita"
                        .CommandType = CommandType.StoredProcedure
                        oParam = objAccesso.GetParameter("@CMNT_id", RandomCode, ParameterDirection.Input, DbType.Int32)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_idpadre", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_nome", "", ParameterDirection.Output, DbType.String, False, 200)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_datacreazione", "", ParameterDirection.Output, DbType.DateTime, True)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_datacessazione", "", ParameterDirection.Output, DbType.DateTime, True)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_dataInizioIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_dataFineIscrizione", "", ParameterDirection.Output, DbType.DateTime, True)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_statuto", "", ParameterDirection.Output, DbType.String, True, 4000)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_mdcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_tpcm_id", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_orgn_id", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_ischiusa", "", ParameterDirection.Output, DbType.Boolean, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, DbType.String, False, 100)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_tpcm_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_orgn_ragSoc", "", ParameterDirection.Output, DbType.String, False, 100)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_PRSN_ID", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_livello", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_CanSubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_CanUnsubscribe", "", ParameterDirection.Output, DbType.Int32, False)
                        .Parameters.Add(oParam)
                        oParam = objAccesso.GetParameter("@CMNT_MaxIscrittiOverList", "", ParameterDirection.Output, DbType.Int32, False)
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
                        Me.n_CMNT_id = RandomCode
                        Me.n_CMNT_idpadre = oRequest.GetValueFromParameter(2)
                        Me.n_CMNT_nome = oRequest.GetValueFromParameter(3)
                        Me.n_CMNT_datacreazione = oRequest.GetValueFromParameter(4)
                        If oRequest.GetValueFromParameter(5) <> "" Then
                            Me.n_CMNT_datacessazione = oRequest.GetValueFromParameter(5)
                        End If
                        If oRequest.GetValueFromParameter(6) <> "" Then
                            Me.n_CMNT_dataInizioIscrizione = oRequest.GetValueFromParameter(6)
                        End If
                        If oRequest.GetValueFromParameter(7) <> "" Then
                            Me.n_CMNT_dataFineIscrizione = oRequest.GetValueFromParameter(7)
                        End If

                        Me.n_CMNT_statuto = oRequest.GetValueFromParameter(8)
                        Me.n_CMNT_mdcm.Id = oRequest.GetValueFromParameter(9)
                        Me.n_CMNT_tpcm.ID = oRequest.GetValueFromParameter(10)

                        Me.n_CMNT_orgn.Id = oRequest.GetValueFromParameter(11)
                        Me.n_CMNT_ischiusa = oRequest.GetValueFromParameter(12)
                        Me.n_CMNT_tpcm.Icona = oRequest.GetValueFromParameter(13)
                        Me.n_CMNT_tpcm.Descrizione = oRequest.GetValueFromParameter(14)
                        Me.n_CMNT_orgn.RagioneSociale = oRequest.GetValueFromParameter(15)
                        Me.n_CMNT_PRSN_ID = oRequest.GetValueFromParameter(16)
                        Me.n_CMNT_livello = oRequest.GetValueFromParameter(17)
                        Me.n_CMNT_MaxIscritti = oRequest.GetValueFromParameter(18)
                        Me.n_CMNT_CanSubscribe = oRequest.GetValueFromParameter(19)
                        Me.n_CMNT_CanUnsubscribe = oRequest.GetValueFromParameter(20)
                        Me.n_CMNT_MaxIscrittiOverList = oRequest.GetValueFromParameter(21)
                        Me.n_CMNT_Archiviata = oRequest.GetValueFromParameter(22)
                        Me.n_CMNT_Bloccata = oRequest.GetValueFromParameter(23)
                        Me.n_CMNT_AccessoLibero = oRequest.GetValueFromParameter(24)
                        Me.n_CMNT_AccessoCopisteria = oRequest.GetValueFromParameter(25)
                        Me.n_Errore = Errori_Db.None
                    Catch ax As Exception
                        Me.n_CMNT_id = -1
                        Me.n_Errore = Errori_Db.DBReadExist
                    End Try
                Else
                    Me.n_CMNT_id = -1
                    Me.n_Errore = Errori_Db.None
                End If
            Catch ex As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.None
            End Try
        End Sub
        Public Function UrlParametriAccessoNonAutenticato(ByVal oServer As System.Web.HttpServerUtility) As String
            Dim UrlEncoded As String = "?rl="
            Dim RandomCode As String

            RandomCode = COL_Encrypter.GeneraNumeriCasuali(6)
            UrlEncoded &= RandomCode & Me.n_CMNT_id & "&hr="
            UrlEncoded &= oServer.UrlEncode(COL_Encrypter.GenerateHashDigest(Me.n_CMNT_id & "Libero", COL_Encrypter.HashMethod.SHA1))
            Return UrlEncoded
        End Function

        Public Function getPercorsoPrincipale() As String
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String = ""
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_getPercorsoPrincipale"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Percorso", "", ParameterDirection.Output, SqlDbType.VarChar, , 8000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return iResponse
        End Function

        Public Function GetOrganizzazioneID() As Integer
            'ritorna l'id dell'organizzazione a cui appartiene la comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim ORGN_ID As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_Organizzazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ORGN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ORGN_ID = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch
                ORGN_ID = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return ORGN_ID
        End Function
        Public Shared Function GetOrganizzazioneID(ByVal ComunitaId As Integer) As Integer
            'ritorna l'id dell'organizzazione a cui appartiene la comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim ORGN_ID As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_Organizzazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", ComunitaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ORGN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ORGN_ID = oRequest.GetValueFromParameter(2)
            Catch
                ORGN_ID = -1
            End Try
            Return ORGN_ID
        End Function


#Region "NUOVA GESTIONE SERVIZI"
        Public Shared Sub DefinisciServiziStandardONLYforAdministration(ByVal ComunitaID As Integer, ByVal ListaServizi As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DefinisciServiziStandard"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, False, 8000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(ComunitaID))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(ComunitaID))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(ComunitaID))
            Catch ax As Exception

            End Try
        End Sub
        Public Sub DefinisciServiziDefault(ByVal ListaServizi As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DefinisciServiziDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, False, 8000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(Me.n_CMNT_id))
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub AssociaProfiloServizi(ByVal PersonaID As Integer, ByVal ProfiloID As Integer, ByVal ListaServizi As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AssociaProfiloServizi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ProfiloID", ProfiloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, False, 8000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(Me.n_CMNT_id))
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Function GetProfiloServizioID() As Integer
            'ritorna l'id dell'organizzazione a cui appartiene la comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim ProfiloID As Integer = 0
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetProfiloServizioID"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ProfiloID", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ProfiloID = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                ProfiloID = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return ProfiloID
        End Function
        Public Function ElencoPossibiliProfili(ByVal PersonaID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencoPossibiliProfili"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet
                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function

        Public Sub DefinisciServiziPermessiDiSistema()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DefinisciServiziPermessiDiSistema"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(Me.n_CMNT_id))
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub DefinisciServiziPermessiProfiloPersonale(ByVal PersonaID As Integer, ByVal ProfiloID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DefinisciServiziPermessiProfiloPersonale"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ProfiloID", ProfiloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(Me.n_CMNT_id))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(Me.n_CMNT_id))
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Function ElencaRuoliPermessi_Definiti(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As DataSet
            'gli id che gli passo sono di servizi e li aggiunge
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliPermessi_Definiti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ServizioID", ServizioID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@OrganizzazioneID", Me.n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TipoComunitaID", Me.TipoComunita.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet
                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
        Public Function ElencaRuoliPermessi_ListaCompleta(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As DataSet
            'gli id che gli passo sono di servizi e li aggiunge
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliPermessi_ListaCompleta"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ServizioID", ServizioID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@OrganizzazioneID", Me.n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TipoComunitaID", Me.TipoComunita.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet
                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
        Public Function ElencaRuoliPermessi_ListaDefault(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As DataSet
            'gli id che gli passo sono di servizi e li aggiunge
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliPermessi_ListaDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ServizioID", ServizioID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@OrganizzazioneID", Me.n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TipoComunitaID", Me.TipoComunita.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet
                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
        'Public Function ElencaRuoliUtenti(ByVal LinguaID As Integer) As DataSet
        '    'gli id che gli passo sono di servizi e li aggiunge
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    Dim iResponse As New DataSet

        '    With oRequest
        '        .Command = "sp_Comunita_ElencaRuoliUtenti"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        iResponse = objAccesso.GetdataSet(oRequest)
        '        n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        iResponse = New DataSet
        '        n_Errore = Errori_Db.DBError
        '    End Try
        '    Return iResponse
        'End Function
        Public Function ElencaRuoliUtentiPermessi(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As DataSet
            'gli id che gli passo sono di servizi e li aggiunge
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliUtentiPermessi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ServizioID", ServizioID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet
                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
#End Region

#Region "Gestione ClonaDiario"

        Public Shared Function InfoDiario(ByVal ComunitaSorgente As Integer, ByRef totaleVisibili As Integer, ByRef totaleNonVisibili As Integer, ByRef totaleLezioni As Integer, ByRef dataPrimaLezione As DateTime) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_InfoDiario"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_StartID", ComunitaSorgente, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totaleVisibili", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totaleNonVisibili", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totaleLezioni", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@dataPrimaLezione", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                totaleVisibili = oRequest.GetValueFromParameter(2)
                totaleNonVisibili = oRequest.GetValueFromParameter(3)
                totaleLezioni = oRequest.GetValueFromParameter(4)

                If oRequest.GetValueFromParameter(5) <> "" Then
                    Try
                        If Equals(CDate(oRequest.GetValueFromParameter(5)), New Date) = False Then
                            dataPrimaLezione = CDate(oRequest.GetValueFromParameter(5))
                        End If
                    Catch ex As Exception

                    End Try
                End If

                Return True
            Catch ax As Exception
                Return False
            End Try
        End Function

        Public Shared Function DuplicaDiario(ByVal ComunitaSorgente As Integer, ByVal ComunitaDestinazione As Integer, ByVal PRSN_ID As Integer, ByVal Visualizza As Integer, ByVal annoAccademico As Integer, ByVal AddDay As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DuplicaDiario"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_StartID", ComunitaSorgente, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_DestinazioneID", ComunitaDestinazione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@Visualizza", Visualizza, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@annoAccademico", annoAccademico, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@AddDay", AddDay, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ax As Exception
                Return False
            End Try
        End Function
#End Region

#Region "Metodi File"
        Public Shared Function HasNewFile(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean, ByVal oFiltroVisibilita As Main.FiltroVisibilità) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasNewFile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                If isLoggato Then
                    oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroVisibilita", CType(oFiltroVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@HasNewFile", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(5) = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function HasLostFile(ByVal CMNT_Id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasLostFile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@HasLostFile", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(2) = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
#End Region
#Region "Metodi associazione Servizi"

        Public Function ElencaRuoli_PermessiForServizio(ByVal SRVZ_ID As Integer, ByVal LinguaID As Integer, Optional ByVal Completi As Boolean = False) As DataSet
            'gli id che gli passo sono di servizi e li aggiunge
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As New DataSet

            With oRequest
                .Command = "sp_Comunita_ElencaRuoli_PermessiForServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ORGN_ID", Me.n_CMNT_orgn.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_TPCM_ID", Me.TipoComunita.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If Completi Then
                    oParam = objAccesso.GetParameter("@Completi", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Completi", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = New DataSet

                n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function



        Public Function ElencaServizi(ByVal LinguaID As Integer) As DataSet

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaServizi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function

        'Public Function AttivaServizio(ByVal SRVZ_ID As Integer)
        '    'gli id che gli passo sono di servizi e li aggiunge
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_AttivaServizio"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch ex As Exception
        '        n_Errore = Errori_Db.DBChange
        '    End Try
        'End Function

        'Public Function DisAttivaServizio(ByVal SRVZ_ID As Integer)
        '    'gli id che gli passo sono di servizi e li aggiunge
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_DisAttivaServizio"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch ex As Exception
        '        n_Errore = Errori_Db.DBChange
        '    End Try
        'End Function

        'Public Function AssociaServizio(ByVal SRVZ_ID As Integer)
        '    'gli id che gli passo sono di servizi e li aggiunge
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_AssociaServizio"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch ex As Exception
        '        n_Errore = Errori_Db.DBInsert
        '    End Try
        'End Function

        'Public Function DisAssociaServizio(ByVal SRVZ_ID As Integer)
        '    'gli id che gli passo sono di servizi e li aggiunge
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_DisAssociaServizio"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch ex As Exception
        '        n_Errore = Errori_Db.DBDelete
        '    End Try
        'End Function

        'Questa funzione restituisce i permessi associati ad un determinato servizio.
        Public Shared Function GetPermessiForServizio(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer, ByVal SRVZ_ID As Integer) As String
            'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetPermessiForServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_ID", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Permessi", "", ParameterDirection.Output, DbType.String, , 32)
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

        'Public Function GetPermessiForServizioByCode(ByVal RuoloID As Integer, ByVal Code As String) As String
        '    'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iResponse As String
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_GetPermessiForServizioByCode"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@TPRL_Id", RuoloID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Code", Code, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Permessi", "", ParameterDirection.Output, DbType.String, , 32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        iResponse = oRequest.GetValueFromParameter(4)
        '    Catch ex As Exception
        '        iResponse = "00000000000000000000000000000000"
        '    End Try
        '    Return iResponse
        'End Function
        Public Shared Function GetPermessiForServizioByCode(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer, ByVal Code As String) As String
            'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetPermessiForServizioByCode"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Code", Code, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Permessi", "", ParameterDirection.Output, DbType.String, , 32)
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
        Public Function GetPermessiForServizioByCode(ByVal RuoloID As Integer, ByVal Code As String) As String
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetPermessiForServizioByCode"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", RuoloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Code", Code, ParameterDirection.Input, SqlDbType.VarChar, , 50)
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
        Public Shared Function GetPermessiForServizioByPersona(ByVal PRSN_ID As Integer, ByVal CMNT_ID As Integer, ByVal SRVZ_Code As String) As String
            'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetPermessiForServizioByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_codice", SRVZ_Code, ParameterDirection.Input, DbType.String, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Permessi", "", ParameterDirection.Output, DbType.String, , 32)
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

        Public Shared Function ChangePermessiForRuolo(ByVal RoleID As Integer, ByVal CMNT_ID As Integer, ByVal SRVZ_ID As Integer, ByVal Permessi As String) As String
            'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ChangePermessiForRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", RoleID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_ID", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Permessi", Permessi, ParameterDirection.Input, DbType.String, , 32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(CMNT_ID, RoleID))
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(CMNT_ID, RoleID))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(CMNT_ID, RoleID))
            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function
        Public Shared Sub PurgeServiceCache(ByVal ServiceID As Integer)
            Dim oService As COL_Servizio = New COL_Servizio(ServiceID)
            oService.EstraiByLingua(1)
            If IsNothing(oService) = False Then
                ObjectBase.PurgeCacheItems(CachePolicy.PermessiServizioUtente(oService.Codice))
            End If
        End Sub

        Public Function ElencaByPermesso(ByVal IdServizio As Integer, ByVal IdPersona As Integer, ByVal Posizione As Integer, Optional ByVal Abilitazione As Integer = 1, Optional ByVal Attivato As Integer = 1, Optional ByVal Abilitato As Integer = 1, Optional ByVal Valore As Integer = 1, Optional ByVal SRVZ_Attivato As Integer = 1) As DataSet
            'mi ritorna l'elenco delle comunità nelle quali ho un determinato permesso passato in ingresso
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaByPermesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_ID", IdServizio, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", IdPersona, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_posizione", Posizione, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitazione", Abilitazione, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Attivato", Attivato, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitato", Abilitato, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_attivato", SRVZ_Attivato, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)

                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable

        End Function

        Public Function ElencaPadriDirettiByPermesso(ByVal IdServizio As Integer, ByVal IdPersona As Integer, ByVal Posizione As Integer, ByVal IdComunita As Integer, Optional ByVal Abilitazione As Integer = 1, Optional ByVal Attivato As Integer = 1, Optional ByVal Abilitato As Integer = 1, Optional ByVal Valore As Integer = 1, Optional ByVal SRVZ_Attivato As Integer = 1) As DataSet
            'mi ritorna l'elenco delle comunità padri nelle quali ho un determinato permesso che non sono già associate come padre alla comunità attuale
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaLivelloSuperiore_ByPermesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_ID", IdServizio, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", IdPersona, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", IdComunita, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_posizione", Posizione, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitazione", Abilitazione, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Attivato", Attivato, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitato", Abilitato, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_attivato", SRVZ_Attivato, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)

                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable

        End Function

        Public Function ElencaPossibiliPadriByServizio(ByVal CodiceServizio As String, ByVal IdPersona As Integer, ByVal Posizione As Integer, ByVal IdComunita As Integer, ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer, ByVal TPCS_ID As Integer, Optional ByVal PeriodoID As Integer = -1, Optional ByVal AnnoAcc As Integer = -1, Optional ByVal isAttivato As Boolean = True, Optional ByVal isAbilitato As Boolean = True, Optional ByVal Valore As Boolean = True, Optional ByVal isServizioAttivo As Boolean = True) As DataSet
            'mi ritorna l'elenco delle comunità padri nelle quali ho un determinato permesso che non sono già associate come padre alla comunità attuale
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaPossibiliPadri_ByServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_Codice", CodiceServizio, , DbType.String, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", IdPersona, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", IdComunita, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_posizione", Posizione, , DbType.Int32)
                .Parameters.Add(oParam)
                If isAttivato Then
                    oParam = objAccesso.GetParameter("@Attivato", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Attivato", 0, , DbType.Int32)
                End If
                .Parameters.Add(oParam)

                If isAbilitato Then
                    oParam = objAccesso.GetParameter("@Abilitato", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Abilitato", 0, , DbType.Int32)
                End If
                .Parameters.Add(oParam)

                If Valore Then
                    oParam = objAccesso.GetParameter("@Valore", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Valore", 0, , DbType.Int32)
                End If

                .Parameters.Add(oParam)

                If isServizioAttivo Then
                    oParam = objAccesso.GetParameter("@SRVZ_attivato", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@SRVZ_attivato", 0, , DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCS_ID", TPCS_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRDO_ID", PeriodoID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Anno", AnnoAcc, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)

                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable

        End Function
#End Region

#Region "Metodi Recupero Iscritti 21/02/2005"
        Public Function GetTotaleIscritti(ByVal oFiltroAbilitazione As Main.FiltroAbilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti, ByVal EsclusaPersonaID As Integer, ByVal InSottocomunita As Boolean, ByVal Percorso As String) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim NumeroIscritti As Integer
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Comunita_TotaleIscritti"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If InSottocomunita Then
                    oParam = objAccesso.GetAdvancedParameter("@InSottocomunita", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@InSottocomunita", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@isDefault", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Percorso", Percorso, ParameterDirection.Input, SqlDbType.VarChar, True, 8000)
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
        Public Function GetTotaleIscrittiForRuoloDefault(ByVal oFiltroAbilitazione As Main.FiltroAbilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim NumeroIscritti As Integer
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Comunita_TotaleIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", -1, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@InSottocomunita", 0, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_ID", 0, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@isDefault", 1, ParameterDirection.Input, DbType.Int32)
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

        Public Function RuoliAssociatiByIscrizione(ByVal LinguaID As Integer, Optional ByVal oFiltro As FiltroRuoli = Main.FiltroRuoli.ForAll_NoGuest, Optional ByVal EsclusaPersonaID As Integer = -1, Optional ByVal oFiltroAbilitazione As Main.FiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato, Optional ByVal oFiltroUtenti As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori, Optional ByVal InSottocomunita As Boolean = False) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_RuoliAssociatiByIscrizione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If InSottocomunita Then
                    oParam = objAccesso.GetAdvancedParameter("@InSottocomunita", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@InSottocomunita", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@forSMS", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
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
        Public Function GetRuoliAssociatiForSMS(ByVal LinguaID As Integer, ByVal EsclusaPersonaID As Integer, ByVal oFiltroAbilitazione As Main.FiltroAbilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti, ByVal InSottocomunita As Boolean) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RuoliAssociatiAdIscritti"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)


                If InSottocomunita Then
                    oParam = objAccesso.GetParameter("@InSottocomunita", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@InSottocomunita", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@forSMS", 1, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
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


        Public Function GetIscrittiByRuolo(ByVal TPRL_ID As Integer, ByVal oFiltroAbilitazione As Abilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti, ByVal InSottocomunita As Boolean, ByVal forSMS As Boolean, Optional ByVal EsclusaPersonaID As Integer = -1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_IscrittiByRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If InSottocomunita Then
                    oParam = objAccesso.GetParameter("@InSottocomunita", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@InSottocomunita", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If forSMS Then
                    oParam = objAccesso.GetParameter("@forSMS", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@forSMS", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_Errore = Errori_Db.DBReadExist
            End Try
            Return dstable
        End Function
        Public Function GetIscrittiByRuolo(ByVal TPRL_ID As Integer, ByVal oFiltroAbilitazione As Abilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti, ByVal InSottocomunita As Boolean, ByVal forSMS As Boolean, ByVal EsclusaPersonaID As Integer, ByVal IDcomunita As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_IscrittiByRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", IDcomunita, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If InSottocomunita Then
                    oParam = objAccesso.GetParameter("@InSottocomunita", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@InSottocomunita", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If forSMS Then
                    oParam = objAccesso.GetParameter("@forSMS", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@forSMS", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_Errore = Errori_Db.DBReadExist
            End Try
            Return dstable
        End Function

        'Public Function GetIscrittiByRuolo(ByVal oTipoRuolo As COL_TipoRuolo, ByVal abilitato As Abilitazione) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Comunita_IscrittiByRuolo"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPRL_ID", oTipoRuolo.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Abilitazione", CType(abilitato, Abilitazione), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        n_Errore = Errori_Db.DBReadExist
        '    End Try
        '    Return dstable
        'End Function
        'Public Function GetIscrittiByRuolo(ByVal oTipoRuolo As COL_TipoRuolo, ByVal oPersona As COL_Persona, ByVal abilitato As Abilitazione) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_IscrittiByRuoloNoUser"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_ID", oPersona.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPRL_ID", oTipoRuolo.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Abilitazione", CType(abilitato, Abilitazione), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try
        '    Return dstable
        'End Function

        'Public Function GetIscrittiByRuoloForSMS(ByVal TPRL_ID As Integer, ByVal PRSN_ID As Integer, ByVal pCMNT_ID As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Comunita_IscrittiByRuoloNoUserForSMS"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CMNT_ID", pCMNT_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try
        '    Return dstable
        'End Function

        Public Function GetIscrittiEsclusiRuoli(ByVal RuoliEsclusi As String, ByVal IDcomunita As Integer, ByVal oFiltroAbilitazione As Abilitazione, ByVal oFiltroUtenti As Main.FiltroUtenti, ByVal InSottocomunita As Boolean, ByVal forSMS As Boolean, Optional ByVal EsclusaPersonaID As Integer = -1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_IscrittiEsclusiRuoli"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ID", IDcomunita, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RuoliEsclusi", RuoliEsclusi, ParameterDirection.Input, DbType.String, True, 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If InSottocomunita Then
                    oParam = objAccesso.GetParameter("@InSottocomunita", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@InSottocomunita", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If forSMS Then
                    oParam = objAccesso.GetParameter("@forSMS", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@forSMS", 0, ParameterDirection.Input, DbType.Int32)
                End If
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


#End Region

#Region "Metodi Gestione Iscritti"
        'Funzione Per modificare il numero degli iscritti !!!
        Public Function SetMaxIscritti(ByVal MaxIscritti As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Boolean = False

            With oRequest
                .Command = "sp_Comunita_SetMaxIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                If Not (MaxIscritti > 0) Then
                    MaxIscritti = 0
                End If
                oParam = objAccesso.GetParameter("@CMNT_MaxIscritti", MaxIscritti, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = True
            Catch ex As Exception

                'gestione errori delle shared!!!
            End Try
            Return iResponse
        End Function
        'Funzione che indica,dato un tipo ruolo, quanti iscritti vi sono per la comunità

        'aggiunge l'utente in fase di registrazione alla super-comunità di livello 0 corrispondente
        'all'organizzazione selezionata
        Public Shared Function AggiungiPersonaOrganizzazione(ByVal PRSN_ID As Integer, ByVal TipoPersona As Integer, ByVal ORGN_ID As Integer, ByVal isAttivato As Boolean, Optional ByVal isAbilitato As Boolean = True, Optional ByVal isResponsabile As Boolean = False, Optional ByVal RicezioneSMS As Integer = 0) As Integer
            'aggiunge l'utente in fase di registrazione alla super-comunità di livello 0 corrispondente
            'all'organizzazione selezionata

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_Organizzazione_IscriviPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_tipo", TipoPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If isAttivato Then
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)
                If isAbilitato Then
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
                oParam = objAccesso.GetParameter("@RLPC_RicezioneSMS", RicezioneSMS, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Return -1
                'gestione errori delle shared!!!
            End Try
            Return -1
        End Function


        'quelli che passo li attiva 
        Public Sub AttivaIscritti(ByVal ElencoIscritti As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AttivaIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Condition", ElencoIscritti, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub AttivaIscritto(ByVal PRSN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AttivaIscritto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        'quelli che passo li abilita
        Public Sub AbilitaIscritti(ByVal ElencoIscritti As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AbilitaIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Condition", ElencoIscritti, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DisabilitaIscritti(ByVal ElencoIscritti As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_DisabilitaIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Condition", ElencoIscritti, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Sub IscriviUtente(ByVal PRSN_ID As Integer, ByVal TPRL_id As Integer, Optional ByVal Attivato As Boolean = True, Optional ByVal Abilitato As Boolean = True, Optional ByVal Responsabile As Boolean = False)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AggiungiUtente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_id", TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If Abilitato Then
                    oParam = objAccesso.GetParameter("@Abilitato", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Abilitato", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)
                If Attivato Then
                    oParam = objAccesso.GetParameter("@Attivato", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Attivato", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)
                If Responsabile Then
                    oParam = objAccesso.GetParameter("@Responsabile", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Responsabile", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub


        'passo l'id utente che verra iscritti ad una comunità come passanti
        Public Sub IscriviUtenteComePassante(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AggiungiUtenteComePassante"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Function IscriviPassanteAcomunita(ByVal PRSN_id As Integer, ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AggiungiUtenteComePassante"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
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

        Public Sub EliminaUtentiInAttesa(ByVal ElencoIscritti As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_EliminaUtentiInAttesa"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ElencoIscritti", ElencoIscritti, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function HasWaitingUsers(Optional ByVal TypeIscription As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_HasWaitingUsers"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TypeIscription", CType(TypeIscription, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
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
            Return False
        End Function
        Public Function HasNewUsers(ByVal PRSN_Id As Integer, ByVal IsConnesso As Boolean, Optional ByVal TypeIscription As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasNewUsers"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TypeIscription", CType(TypeIscription, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If IsConnesso Then
                    oParam = objAccesso.GetAdvancedParameter("@IsConnesso", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@IsConnesso", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
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
            Return False
        End Function
        Public Function HasBlockedUsers(Optional ByVal TypeIscription As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasBlockedUsers"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TypeIscription", CType(TypeIscription, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
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
            Return False
        End Function
        Public Function HasNoActivatedUsers(Optional ByVal TypeIscription As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_HasNoActivatedUsers"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TypeIscription", CType(TypeIscription, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
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
            Return False
        End Function
#End Region


#Region "Metodi Gestione Ruoli"
        Public Function RuoloByComunitaAndPersona(ByVal IdPersona As Integer) As Integer 'agg al: 30/10/2003
            'ritorna il ruolo della persona in quella comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim IdRuolo As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoRuolo_EstraiByComunitaAndPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", IdPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del ruolo della persona
                IdRuolo = oRequest.GetValueFromParameter(3)
            Catch ex As Exception
                IdRuolo = 0
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return IdRuolo
        End Function

        Public Function RuoloDefault() As Integer
            'restituisce il tipo ruolo di default con cui c si iscrive ad alla comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim TipoRuolo As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ComunitaRuoloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID del tipo di ruolo di default per quel tipo di comunità
                TipoRuolo = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                TipoRuolo = -1
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return TipoRuolo
        End Function
        Public Function RuoliAssociati(ByVal LinguaID As Integer, ByVal oFiltro As FiltroRuoli, Optional ByVal oFiltroUtenti As FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori, Optional ByVal EsclusoUtenteID As Integer = 0) As DataSet
            'elenca in un dataset i ruoli che sono assegnati ad una comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(oFiltroUtenti, FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusoUtenteID", EsclusoUtenteID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oTable
        End Function
        Public Function RuoliAssociabiliByPersona(ByVal PRSN_Id As Integer, ByVal oFiltro As FiltroRuoli, Optional ByVal oFiltroUtenti As FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As DataSet
            'elenca in un dataset i ruoli che sono assegnati ad una comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliAssociabiliByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(oFiltroUtenti, FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oTable
        End Function
#End Region



#Region "Import Data"
        Public Shared Function Ruoli_EsportaNoIscritti(ByVal IdSorgente As Integer, ByVal idDestinatario As Integer, ByVal oAbilitazione As Main.FiltroAbilitazione, ByVal oFiltroUtente As Main.FiltroUtenti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_Ruoli_EsportaNoIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@IdSorgente", IdSorgente, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idDestinatario", idDestinatario, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroUtente", CType(oFiltroUtente, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                oDataSet = New DataSet
            End Try
            Return oDataSet
        End Function
        Public Shared Function ImportaNoIscrittiByRuolo(ByVal IdSorgente As Integer, ByVal idDestinatario As Integer, ByVal TPRLsorgente_ID As Integer, ByVal TPRLdestintario_ID As Integer, ByVal oAbilitazione As Main.FiltroAbilitazione) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ImportaNoIscrittiByRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@totaleImport", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@IdSorgente", IdSorgente, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idDestinatario", idDestinatario, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRLsorgente_ID", TPRLsorgente_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRLdestintario_ID", TPRLdestintario_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String, 30)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(0)
            Catch ex As Exception
                iResponse = 0
            End Try
            Return iResponse
        End Function
        Public Shared Function ImportaComePassanteNoIscrittiByRuolo(ByVal IdSorgente As Integer, ByVal idDestinatario As Integer, ByVal TPRLsorgente_ID As Integer, ByVal IDpassante As Integer, ByVal IsChiusaPassante As Boolean, ByVal oAbilitazione As Main.FiltroAbilitazione) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ImportaComePassanteNoIscrittiByRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@totaleImport", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@IdSorgente", IdSorgente, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idDestinatario", idDestinatario, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRLsorgente_ID", TPRLsorgente_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@IDpassante", IDpassante, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If IsChiusaPassante Then
                    oParam = objAccesso.GetParameter("@IsChiusaPassante", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@IsChiusaPassante", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)



                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(0)
            Catch ex As Exception
                iResponse = 0
            End Try
            Return iResponse
        End Function
#End Region

#Region "Metodi"
        'I metodi pubblici sono quasi tutti Overridable o Shared.
        'I metodi Protected sono utilizzati da quelli pubblici

        Public Overridable Function IsInserita() As Boolean
            Return ControlloEsistenza("select CMNT_id from Comunita where CMNT_id=" & Me.n_CMNT_id)
        End Function

        Public Shared Function ElencaComunita(ByVal IdOrganizzazione As Integer, ByVal IdPadre As Integer, ByVal TipoComunita As Integer) As DataSet
            'manca gestione errore (shared)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", IdOrganizzazione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_id", TipoComunita, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idPadre", IdPadre, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                'gestione errore metodo shared mancante
            End Try
            Return dsTable
        End Function


        Public Function ElencaNonIscritti_MaIscrittiACmntPassata(ByVal LinguaID As Integer, ByVal TipoRuolo As Integer, ByVal CMNT_id_passato As Integer, Optional ByVal valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oFiltroUtente As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori, Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti, Optional ByVal pFilter As ProfileStatusFilter = ProfileStatusFilter.all) As DataSet
            'elenca gli iscritti ad una comunità passata ma non alla corrente
            'con TipoRuolo = -1 elenca gli utenti senza filtraggio per tipologia
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaNonIscritti_MaIscrittiACmntPassata"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id_passato", CMNT_id_passato, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_id", TipoRuolo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtente, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroRicerca", CType(oFiltroRicerca, Main.FiltroRicercaAnagrafica), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@status", CInt(pFilter), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oTable
        End Function

        Public Function ElencaNonIscritti_BySistema(ByVal LinguaID As Integer, ByVal TipoPersona As Integer, Optional ByVal valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti, Optional ByVal pFilter As ProfileStatusFilter = ProfileStatusFilter.all) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaNonIscritti_BySistema"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPPR_id", TipoPersona, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@status", CInt(pFilter), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@FiltroRicerca", CType(oFiltroRicerca, FiltroRicercaAnagrafica), ParameterDirection.Input, SqlDbType.Int)
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


        Public Function ElencaIscrittiNoMittente(ByVal LinguaID As Integer, ByVal IdPersona As Integer, ByVal oFiltroAbilitazione As FiltroAbilitazione, ByVal oFiltroUtente As FiltroUtenti, Optional ByVal TPRL_id As Integer = -1, Optional ByVal pPAGE_SIZE As Integer = -1, Optional ByVal pPAGE_INDEX As Integer = -1, Optional ByVal Valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oCampoOrdine As FiltroCampoOrdine = FiltroCampoOrdine.anagrafica, Optional ByVal WithFoto As Boolean = False, Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti) As DataSet

            Dim str As String = ""

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaIscrittiNoMittente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", IdPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitazione", CType(oFiltroAbilitazione, FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, Main.FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinamento", CType(oOrdinamento, FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinaPer", CType(oCampoOrdine, FiltroCampoOrdine), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtente, FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If WithFoto Then
                    oParam = objAccesso.GetParameter("@WithFoto", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@WithFoto", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroRicerca", CType(oFiltroRicerca, Main.FiltroRicercaAnagrafica), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                Dim dt1 As DateTime
                Dim dt2 As TimeSpan

                dt1 = Now

                oTable = objAccesso.GetdataSet(oRequest)

                dt2 = Now - dt1
                str = dt2.ToString & vbCrLf

            Catch ex As Exception
                n_Errore = Errori_Db.DBError
            End Try
            Return oTable
        End Function

        Public Function ElencaIscrittiNoMittenteEsclusiGruppi(ByVal LinguaID As Integer, ByVal IdPersona As Integer, ByVal oFiltroAbilitazione As FiltroAbilitazione, ByVal oFiltroUtente As FiltroUtenti, ByVal GruppiEsclusi As String, Optional ByVal pPAGE_SIZE As Integer = -1, Optional ByVal pPAGE_INDEX As Integer = -1, Optional ByVal Valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oCampoOrdine As FiltroCampoOrdine = FiltroCampoOrdine.anagrafica, Optional ByVal WithFoto As Boolean = False, Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaIscrittiNoMittenteEsclusiGruppi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", IdPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitazione", CType(oFiltroAbilitazione, FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinamento", CType(oOrdinamento, FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinaPer", CType(oCampoOrdine, FiltroCampoOrdine), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@GruppiEsclusi", GruppiEsclusi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtente, FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If WithFoto Then
                    oParam = objAccesso.GetParameter("@WithFoto", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@WithFoto", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@FiltroRicerca", CType(oFiltroRicerca, Main.FiltroRicercaAnagrafica), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try

                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_Errore = Errori_Db.DBError
            End Try
            Return oTable
        End Function

        Protected Function ControlloEsistenza(ByVal stringaSQL As String) As Boolean
            'VIVALDI... rivolgersi a lui...
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = stringaSQL
                .CommandType = CommandType.Text
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            If dsTable.Tables(0).Rows.Count = 1 Then
                Return True
            Else
                Return False
            End If
        End Function



        Public Sub EliminaServizio(ByVal Condition As String)
            'gli id che gli passo sono di servizi e li elimina
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_EliminaServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKSC_CMNT_id", n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Condition", Condition, ParameterDirection.Input, DbType.String, False, 4000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                n_Errore = Errori_Db.DBDelete
            End Try
        End Sub

        Public Function ElencaOrganizzazioni(ByVal idISTT As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaOrganizzazioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_id", idISTT, ParameterDirection.Input, DbType.Int32)
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

#End Region

#Region "Metodi SMS"
        'Public Function EstraiLimiteSMS() As COL_Limite_SMS
        '    Dim oTable As new DataSet
        '    Dim oLimite As New COL_Limite_SMS
        '    otable = Me.GetLimiteSMSDefault

        '    If otable.HasErrors Then
        '        oLimite.Errore = Errori_Db.DBError
        '    ElseIf otable.Tables(0).Rows.Count >= 1 Then
        '        If IsDBNull(otable.Tables(0).Rows(0).Item("LMSM_Utilizzati")) Then
        '            oLimite.Utilizzati = 0
        '        Else
        '            oLimite.Utilizzati = otable.Tables(0).Rows(0).Item("LMSM_Utilizzati")
        '        End If
        '        If IsDBNull(otable.Tables(0).Rows(0).Item("LMSM_Disponibili")) Then
        '            oLimite.Disponibili = 0
        '        Else
        '            oLimite.Disponibili = otable.Tables(0).Rows(0).Item("LMSM_Disponibili")
        '        End If
        '        If IsDBNull(otable.Tables(0).Rows(0).Item("TPLM_Descrizione")) Then
        '            oLimite.Tipo_Limite.Descrizione = ""
        '        Else
        '            oLimite.Tipo_Limite.Descrizione = otable.Tables(0).Rows(0).Item("TPLM_Descrizione")
        '        End If
        '        oLimite.Tipo_Limite.ID = otable.Tables(0).Rows(0).Item("TPLM_ID")
        '        oLimite.Errore = Errori_Db.None
        '    Else
        '        oLimite.Errore = Errori_Db.DBError
        '    End If
        '    Return oLimite
        'End Function

        'Public Function GetLimiteSMSDefault() As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String
        '    Dim oDataAttuale As DateTime

        '    With oRequest
        '        .Command = "sp_Comunita_GetLimiteSMSDefault"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oDataAttuale = Now
        '        oData = Format(Now, "dd/MM/yyyy hh:mm:ss")

        '        oData = oData.Replace(".", ":")
        '        oParam = objAccesso.GetParameter("@DataAttuale", oData, , DbType.DateTime)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.Id = -1
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function

        'Public Function NeedCreditoForInvio() As Boolean
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iDisponibile As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_NeedCreditoForInvio"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@NeedCredito", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    iDisponibile = 0
        '    Try
        '        iDisponibile = objAccesso.GetExecuteNotQuery(oRequest)

        '        iDisponibile = oRequest.GetValueFromParameter(3)
        '        Me.n_Errore = Errori_Db.None
        '        If iDisponibile > 0 Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try

        '    Return False
        'End Function

        'Public Function NeedCreditoForRicezione() As Boolean
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iDisponibile As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_NeedCreditoForRicezione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@NeedCredito", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    iDisponibile = 0
        '    Try
        '        iDisponibile = objAccesso.GetExecuteNotQuery(oRequest)

        '        iDisponibile = oRequest.GetValueFromParameter(3)
        '        Me.n_Errore = Errori_Db.None
        '        If iDisponibile > 0 Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try

        '    Return False
        'End Function

        'Public Function GetConfiguration() As COL_Configurazione_SMS
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iResponse As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oConfiguration As New COL_Configurazione_SMS

        '    With oRequest
        '        .Command = "sp_Comunita_ConfigurazioneSMS"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CNFS_ID", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CNFS_fasciaInvio", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CNFS_fasciaRicezione", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CNFS_Attivo", 0, ParameterDirection.Output, DbType.Byte)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        '        oConfiguration.ID = oRequest.GetValueFromParameter(2)
        '        oConfiguration.Comunita = Me
        '        oConfiguration.Invio = oRequest.GetValueFromParameter(3)
        '        oConfiguration.Ricezione = oRequest.GetValueFromParameter(4)
        '        If oRequest.GetValueFromParameter(5) = 1 Then
        '            oConfiguration.Attivo = True
        '        Else
        '            oConfiguration.Attivo = False
        '        End If
        '        oConfiguration.Errore = Errori_Db.None
        '    Catch ex As Exception
        '        oConfiguration.ID = -1
        '        oConfiguration.Errore = Errori_Db.DBReadExist
        '    End Try

        '    Return oConfiguration
        'End Function

        'Public Function GetLimitiSMSAssociati() As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Comunita_LimitiAssociati"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)

        '        ' Recupero l'ID univoco della persona.
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return dstable
        'End Function

        'Public Function GetLimitiSMSDaAssociare_Associati() As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess


        '    With oRequest
        '        .Command = "sp_Comunita_LimitiDaAssociare_Associati"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)

        '        ' Recupero l'ID univoco della persona.
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return dstable
        'End Function
        'Public Function SetSMSUtilizzati(ByVal oUtilizzati As Integer) As Boolean
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_SMSUtilizzati"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", Me.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@LMSM_Utilizzati", oUtilizzati, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch ex As Exception
        '        Me.Id = -1
        '        Me.n_Errore = Errori_Db.DBError
        '        dstable = -1
        '    End Try
        '    Try
        '        COL_Pacchetto_SMS.ConsumaSMS(oUtilizzati, Me)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBChange
        '    End Try
        '    Return True
        'End Function
#End Region

#Region "RECUPERO ISCRITTI PER RUBRICA !!!!!"

        ' VERIFICARE USO  PER SMS; RICREARE STORE APPOSITE
        Public Function GetTotaleIscrittiByRuoloNoMittenteForSMS(ByVal pTPRL_ID As Integer, ByVal pPRSN_ID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_TotaleIscrittiByRuoloNoMittenteForSMS"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", pPRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", pTPRL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Totale", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function

        Public Function GetTotaleIscrittiByRuoloNoMittenteForSMS(ByVal pTPRL_ID As Integer, ByVal pPRSN_ID As Integer, ByVal pCMNT_ID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_TotaleIscrittiByRuoloNoMittenteForSMS"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ID", pCMNT_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ID", pPRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_ID", pTPRL_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@Totale", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function
        Public Function GetSMSDisponibili(ByVal PRSN_ID As Integer, ByVal CMNT_Padri As String) As Integer
            Dim i_SMSDisponibili As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim otable As Integer
            Dim oAccesso As New COL_DataAccess

            i_SMSDisponibili = 0

            With oRequest
                .Command = "sp_Comunita_GetSMSDisponibiliByPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = oAccesso.GetParameter("@CMNT_Disponibili", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                'data formattata in gg/mm/yyyy HH:mm:ss
                oParam = oAccesso.GetParameter("@CMNT_DataAttuale", DateToString(Now, True), , DbType.String, , 30)
                .Parameters.Add(oParam)

                oParam = oAccesso.GetParameter("@CMNT_ID", Me.n_CMNT_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = oAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = oAccesso.GetParameter("@CMNT_Padri", CMNT_Padri, , DbType.String, , 4000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                otable = oAccesso.GetExecuteNotQuery(oRequest)

                i_SMSDisponibili = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                i_SMSDisponibili = 0
            End Try

            Return i_SMSDisponibili
        End Function
#End Region

#Region "Da controllare/cancellare"
        Public Function NuovaSottocomunita() As COL_Comunita
            Dim Sottocomunita As New COL_Comunita
            Sottocomunita.IdPadre = Me.n_CMNT_id
            Return Sottocomunita
        End Function
#End Region

#Region "Gestione Comunità / Cancellazione"
        Public Shared Function HasDoppioni(ByVal nome As String, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal IdComunita As Integer = -1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasDoppioni"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", IdComunita, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@nome", nome, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@hasDoppioni", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                If oRequest.GetValueFromParameter(4) = 1 Then
                    iResponse = True
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function

        'Public Shared Function ___GeneraAlberoForCancellazione(ByVal CMNT_Id As Integer, ByVal CMNT_Path As String, ByVal CMNT_PadreID As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_GeneraAlbero"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_Path", CMNT_Path, , DbType.String, , 3000)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_PadreID", CMNT_PadreID, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Cancella", 1, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function

        Public Sub GetPadre()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_GetPadre"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_idpadre", "", ParameterDirection.Output, SqlDbType.Int, False)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CMNT_idpadre = oRequest.GetValueFromParameter(2)
            Catch ax As Exception
                Me.n_CMNT_id = -1
                Me.n_CMNT_idpadre = 0
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Function GetPadri(ByVal LinguaID As Integer) As DataSet
            'elenca tutte le comunità padri associate ad una data comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet
            With oRequest
                .Command = "sp_Comunita_GetPadri"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataSet
        End Function
        Public Function GetPadriDiretti() As DataSet
            'elenca tutte le comunità padri associate ad una data comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet
            With oRequest
                .Command = "sp_Comunita_GetPadri"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataSet
        End Function

        Public Shared Function EliminaComunita(ByVal IdComunita As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Comunita_Elimina"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", IdComunita, , DbType.Int32)
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

        Public Shared Function CancellaLinkComunita(ByVal IdComunita As Integer, ByVal IdPadre_Link As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_EliminaLink"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", IdComunita, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_PadreID", IdPadre_Link, , DbType.Int32)
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

        'Public Shared Function ElencaGlobale_Paginate(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ORGN_ID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal TPCM_ID As Integer = -1, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal oFiltro As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal oElencoRecord As Main.ElencoRecord = Main.ElencoRecord.Normale) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_ElencaGlobale_Paginate"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltro, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_INDEX", pageIndex, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_SIZE", pageSize, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@forAlbero", CType(oElencoRecord, Main.ElencoRecord), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function

        Public Shared Function ElencaGlobale_PaginateByPersona(ByVal LinguaID As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ORGN_ID As Integer, ByVal PRSN_ID As Integer, ByVal oFiltroUtenti As FiltroUtenti, Optional ByVal TPCM_ID As Integer = -1, Optional ByVal oFiltro As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TPRL_ID As Integer = -1, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal oElencoRecord As Main.ElencoRecord = Main.ElencoRecord.Normale, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Comunita_ElencaGlobale_Paginate"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltro, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_INDEX", pageIndex, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pageSize, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@forAlbero", CType(oElencoRecord, Main.ElencoRecord), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
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


        Public Shared Function ElencaComunita_ForAdmin(ByVal LinguaID As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ORGN_ID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal TPCM_ID As Integer = -1, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal oFiltroComunita As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoCorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal oElencoRecord As Main.ElencoRecord = Main.ElencoRecord.Normale, Optional ByVal CompleteTree As Boolean = False, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaGlobale_forAdmin"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltroComunita, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_INDEX", pageIndex, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pageSize, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@forAlbero", CType(oElencoRecord, Main.ElencoRecord), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If CompleteTree Then
                    oParam = objAccesso.GetParameter("@CompleteTree", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@CompleteTree", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TipoCorsoDiStudi_Id", TipoCorsoDiStudi_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
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

        Public Shared Function ElencaComunitaPadri_ForClone(ByVal ComunitaID As Integer, ByVal PadreID As Integer, ByVal LinguaID As Integer, Optional ByVal CompleteTree As Boolean = False, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaComunitaPadri_ForClone"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PadreID", PadreID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
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


        Public Shared Function ElencaListaSemplice_ForServizio(ByVal LinguaID As Integer, ByVal SRVZ_Codice As String, ByVal PRSN_Id As Integer, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaListaSemplice_ForServizio"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Codice", SRVZ_Codice, ParameterDirection.Input, SqlDbType.VarChar, , 32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
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

        Public Function RimuoviCollegamentoPadre(ByVal ComunitaPadreID As Integer, ByVal cancellaIscrizioni As Boolean, ByVal PercorsoComunita As String, ByVal PercorsoProfilo As String) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RimuoviCollegamentoPadre"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaPadreID", ComunitaPadreID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If cancellaIscrizioni Then
                    oParam = objAccesso.GetAdvancedParameter("@cancellaIscrizioni", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@cancellaIscrizioni", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)

                If cancellaIscrizioni = False Then
                    Me.RicalcolaIscrizioni(ComunitaPadreID, PercorsoComunita, PercorsoProfilo)
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

        Private Sub RicalcolaIscrizioni(ByVal ComunitaPadreID As Integer, ByVal PercorsoComunita As String, ByVal PercorsoProfilo As String)
            Dim oDatasetIscrizioni As DataSet
            Dim i, totale As Integer
            Dim ElencoIDcomunita() As String

            Try
                oDatasetIscrizioni = Me.RecuperaIscrizioniDaRedifinire(ComunitaPadreID)
                totale = oDatasetIscrizioni.Tables(0).Rows.Count - 1

                ElencoIDcomunita = PercorsoComunita.Split(".")
                For i = 0 To totale
                    Dim PersonaID, totalePadri, j As Integer
                    Dim oPersona As New COL_Persona
                    Dim PercorsoPadri As String

                    PersonaID = oDatasetIscrizioni.Tables(0).Rows(i).Item("RLPC_PRSN_ID")

                    totalePadri = UBound(ElencoIDcomunita) - 1
                    PercorsoPadri = PercorsoComunita
                    For j = totalePadri To 0 Step -1
                        If IsNumeric(ElencoIDcomunita(j)) Then
                            Dim oDataset As DataSet
                            oPersona.ID = PersonaID
                            oDataset = oPersona.VerificaIscrizioneAPadri(ElencoIDcomunita(j), PersonaID)
                            totalePadri = oDataset.Tables(0).Rows.Count
                            If totalePadri > 0 Then
                                Exit For
                            Else
                                If ElencoIDcomunita(j - 1) = "" Then
                                    Exit For
                                Else
                                    Me.IscriviPassanteAcomunita(PersonaID, ElencoIDcomunita(j - 1))
                                    PercorsoPadri = Left(PercorsoPadri, InStr(PercorsoPadri, "." & ElencoIDcomunita(j - 1) & "."))
                                    PercorsoPadri = PercorsoPadri & ElencoIDcomunita(j - 1) & "."
                                    oPersona.AggiornaProfiloXML(ElencoIDcomunita(j - 1), PercorsoPadri & "\" & PersonaID & "\", PercorsoProfilo)
                                End If
                            End If
                        End If
                    Next
                Next
            Catch ex As Exception

            End Try
        End Sub

        Private Function RecuperaIscrizioniDaRedifinire(ByVal ComunitaPadreID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RecuperaIscrizioniDaRedifinire"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaPadreID", ComunitaPadreID, ParameterDirection.Input, SqlDbType.Int)
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

#Region "Info Comunità"
        Public Function GetResponsabile() As COL_Persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oPersona As New COL_Persona
            With oRequest
                .Command = "sp_Comunita_GetResponsabile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", "", ParameterDirection.Output, SqlDbType.Int, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_nome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_cognome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_mail", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, False, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_fotoPath", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                oPersona.ID = oRequest.GetValueFromParameter(2)
                oPersona.Nome = oRequest.GetValueFromParameter(3)
                oPersona.Cognome = oRequest.GetValueFromParameter(4)
                oPersona.Mail = oRequest.GetValueFromParameter(5)
                oPersona.InfoRicevimento = oRequest.GetValueFromParameter(6)
                oPersona.FotoPath = oRequest.GetValueFromParameter(7)
                oPersona.MailSecondaria = oRequest.GetValueFromParameter(8)
                oPersona.Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBReadExist
                oPersona.Errore = Errori_Db.DBReadExist
            End Try
            Return oPersona
        End Function
        Public Function GetCreatore() As COL_Persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oPersona As New COL_Persona
            With oRequest
                .Command = "sp_Comunita_GetCreatore"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", "", ParameterDirection.Output, SqlDbType.Int, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_nome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_cognome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_mail", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, False, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_fotoPath", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                oPersona.ID = oRequest.GetValueFromParameter(2)
                oPersona.Nome = oRequest.GetValueFromParameter(3)
                oPersona.Cognome = oRequest.GetValueFromParameter(4)
                oPersona.Mail = oRequest.GetValueFromParameter(5)
                oPersona.InfoRicevimento = oRequest.GetValueFromParameter(6)
                oPersona.FotoPath = oRequest.GetValueFromParameter(7)
                oPersona.MailSecondaria = oRequest.GetValueFromParameter(8)
                oPersona.Errore = Errori_Db.None
            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBReadExist
                oPersona.Errore = Errori_Db.DBReadExist
            End Try
            Return oPersona
        End Function

        'Public Function GetNomeResponsabile() As String
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    Dim nomeResponsabile As String

        '    With oRequest
        '        .Command = "sp_Comunita_GetResponsabile"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32, False)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_nome", "", ParameterDirection.Output, DbType.String, False, 30)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_cognome", "", ParameterDirection.Output, DbType.String, False, 30)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_mail", "", ParameterDirection.Output, DbType.String, False, 50)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    nomeResponsabile = ""
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        If oRequest.GetValueFromParameter(3) <> "" And oRequest.GetValueFromParameter(4) <> "" Then
        '            nomeResponsabile = oRequest.GetValueFromParameter(4) & " " & oRequest.GetValueFromParameter(3)
        '        ElseIf oRequest.GetValueFromParameter(3) <> "" Then
        '            nomeResponsabile = oRequest.GetValueFromParameter(3)
        '        ElseIf oRequest.GetValueFromParameter(4) <> "" Then
        '            nomeResponsabile = oRequest.GetValueFromParameter(4)
        '        End If
        '        Me.n_Errore = Errori_Db.None
        '    Catch ax As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist

        '    End Try
        '    Return nomeResponsabile
        'End Function
        Public Function GetNomeResponsabile_NomeCreatore() As String
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim anagrafica As String

            With oRequest
                .Command = "sp_Comunita_GetResponsabile_Creatore"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", Me.n_CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagraficaResponsabile", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagraficaCreatore", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            anagrafica = ","
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(2) <> "" And oRequest.GetValueFromParameter(3) <> "" Then
                    anagrafica = Replace(oRequest.GetValueFromParameter(2), ",", " ") & "," & Replace(oRequest.GetValueFromParameter(3), ",", " ")
                ElseIf oRequest.GetValueFromParameter(2) <> "" Then
                    anagrafica = Replace(oRequest.GetValueFromParameter(2), ",", " ") & ","
                ElseIf oRequest.GetValueFromParameter(3) <> "" Then
                    anagrafica = "," & Replace(oRequest.GetValueFromParameter(3), ",", " ")
                End If
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception

                Me.n_Errore = Errori_Db.DBReadExist

            End Try
            Return anagrafica
        End Function

        'Public Shared Function EstraiNome(ByVal ComunitaID As Integer) As String

        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    Dim cmntName As String

        '    With oRequest
        '        .Command = "sp_LogFile_NomeCmnt"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_id", ComunitaID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_nome", "", ParameterDirection.Output, DbType.String, False, 200)
        '        .Parameters.Add(oParam)
        '        'Il parametro che segue serve se si scommantano i commenti nella stored procedure relativa
        '        'praticamente serve per avere di ritorno anche la data e ora di creazione della comunità
        '        'oParam = objAccesso.GetParameter("@CMNT_dataCreazione", "", ParameterDirection.Output, DbType.DateTime, False, 50)
        '        '.Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        cmntName = oRequest.GetValueFromParameter(2).ToString
        '        'Il parametro che segue serve se si scommantano i commenti nella stored procedure relativa
        '        'cmntName = cmntName & Replace(Trim(oRequest.GetValueFromParameter(3).ToString), "/", "-", , , CompareMethod.Text)
        '    Catch ex As Exception
        '        cmntName = ex.ToString
        '        'decidere come gestire gli errori nei metodi shared
        '    End Try
        '    Return cmntName
        'End Function
        Public Function EstraiNomeBylingua(ByVal LinguaID As Integer) As String
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NomeComunita As String = ""

            With oRequest
                .Command = "sp_Comunita_EstraiNomeByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaNome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NomeComunita = oRequest.GetValueFromParameter(2).ToString
            Catch ex As Exception
                NomeComunita = ""
            End Try
            Return NomeComunita
        End Function
        Public Shared Function EstraiNomeBylingua(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As String
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NomeComunita As String = ""

            With oRequest
                .Command = "sp_Comunita_EstraiNomeByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaNome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NomeComunita = oRequest.GetValueFromParameter(2).ToString
            Catch ex As Exception
                NomeComunita = ""
            End Try
            Return NomeComunita
        End Function
        Public Sub MailNotifica(ByVal oPersona As COL_Persona, ByVal link As String, ByVal SessionId As String, ByVal TPRL_Nome As String, ByVal LinguaCode As String, ByVal oLocalizedMail As MailLocalized)

            Dim oResponsabile As New COL_Persona

            'mi ricavo il nome dell'organizzazione di default
            Dim oOrganizzazione As New COL_Organizzazione
            oOrganizzazione.Id = oPersona.GetOrganizzazioneDefault()
            oOrganizzazione.Estrai()

            Try
                oResponsabile = Me.GetResponsabile()
                oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(LinguaCode))
                'oLocalizedMail.ReloadByLanguage(LinguaCode)
                Dim oMail As New COL_E_Mail(oLocalizedMail)


                If oResponsabile.Errore = Errori_Db.None Then
                    oMail.IndirizziTO.Add(New MailAddress(oResponsabile.Mail, oResponsabile.Anagrafica))
                Else
                    Dim oCreatore As New COL_Persona
                    If oCreatore.Errore = Errori_Db.None Then
                        oMail.IndirizziTO.Add(New MailAddress(oCreatore.Mail, oCreatore.Anagrafica))
                    End If
                End If

                While InStr(SessionId, "&") > 0
                    SessionId = Replace(SessionId, "&", "_")
                End While

                Me.Aggiungi_InAttesaConferma(SessionId, oPersona.ID, Me.n_CMNT_id)
                If oMail.IndirizziTO.Count > 0 Then
                    Dim stringaCriptata As String
                    '  Dim oEncrypter As New COL_Encrypter

                    'stringaCriptata = oEncrypter.Encrypt("activate&PRSN_ID=" & oPersona.Id & "&CMNT_ID=" & Me.Id)
                    stringaCriptata = "activate&" & CriptaParametriAttivazione(oPersona.ID, SessionId, Me.Id)

                    'PRSN_ID = " & oPersona.Id & " & LimboID = " & SessionId & " & CMNT_ID = " & Me.n_CMNT_id"

                    oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ComunitySubscription).Subject
                    If Hour(Now) <= 12 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message
                    ElseIf Hour(Now) > 12 And Hour(Now) < 18 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message
                    Else
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message
                    End If
                    oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ComunitySubscription).Message
                    oMail.Body = Replace(oMail.Body, "#data#", FormatDateTime(Now, DateFormat.LongDate))
                    oMail.Body = Replace(oMail.Body, "#username#", oPersona.Cognome & " " & oPersona.Nome)
                    oMail.Body = Replace(oMail.Body, "#facolta#", oOrganizzazione.RagioneSociale)
                    oMail.Body = Replace(oMail.Body, "#usermail#", oPersona.Mail)
                    oMail.Body = Replace(oMail.Body, "#usertipo#", oPersona.TipoPersona.Descrizione)
                    If oPersona.TipoPersona.ID = 4 Then 'se è esterno scrivo la mansione
                        Dim oEsterno As New COL_Esterno
                        oEsterno.ID = oPersona.ID
                        oEsterno.GetMansione()
                        oMail.Body = Replace(oMail.Body, "#mansione#", ", " & oEsterno.Mansione)
                    Else
                        oMail.Body = Replace(oMail.Body, "#mansione#", "")
                    End If
                    oMail.Body = Replace(oMail.Body, "#comunita#", Me.Nome)
                    oMail.Body = Replace(oMail.Body, "#ruolo#", TPRL_Nome)
                    oMail.Body = Replace(oMail.Body, "#link#", link & "activateFromMail.aspx?action=" & stringaCriptata)

                    oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oLocalizedMail.SystemFirmaNotifica

                    oMail.Body = oMail.Body.Replace("<br>", vbCrLf)
                    oMail.Mittente = oLocalizedMail.SystemSender


                    oMail.InviaMail()
                End If
            Catch ex As Exception

            End Try
        End Sub

        Private Function CriptaParametriAttivazione(ByVal PRSN_Id As Integer, ByVal LimboID As String, ByVal CMNT_ID As Integer) As String
            Dim RandomCode As String
            Dim Link As String
            RandomCode = COL_Persona.generaPasswordNumerica(8)

            Link = "AddCode=" & RandomCode & CMNT_ID

            RandomCode = COL_Persona.generaPasswordNumerica(4)
            Link = Link & "&ExpUrl=t" & RandomCode & PRSN_Id

            RandomCode = COL_Persona.generaPasswordNumerica(5)
            Link = Link & "&ExpUrl2=j" & RandomCode & LimboID

            Return Link
        End Function
        Public Sub MailAccettazione(ByVal oPersona As COL_Persona, ByVal oLocalizedMail As MailLocalized)
            Dim oResponsabile As New COL_Persona


            Try
                oResponsabile = Me.GetResponsabile()
                oLocalizedMail = ManagerConfiguration.GetMailLocalized(oPersona.Lingua)
                '	oLocalizedMail.ReloadByLanguage(oPersona.Lingua.Codice)
                Dim oMail As New COL_E_Mail(oLocalizedMail)
                If oResponsabile.Errore = Errori_Db.None Then
                    oMail.IndirizziCC.Add(New MailAddress(oResponsabile.Mail, oResponsabile.Anagrafica))
                Else
                    Dim oCreatore As New COL_Persona
                    oCreatore = Me.GetCreatore
                    If oCreatore.Errore = Errori_Db.None Then
                        oMail.IndirizziCC.Add(New MailAddress(oCreatore.Mail, oCreatore.Anagrafica))
                    End If
                End If

                If oMail.IndirizziCC.Count > 0 Then
                    oMail.Mittente = oLocalizedMail.SystemSender
                    oMail.IndirizziTO.Add(New MailAddress(oPersona.Mail))

                    oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.CommunityAcceptSupscription).Subject
                    If Hour(Now) <= 12 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message
                    ElseIf Hour(Now) > 12 And Hour(Now) < 18 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message
                    Else
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message
                    End If

                    oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.CommunityAcceptSupscription).Message & vbCrLf & vbCrLf & vbCrLf
                    oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
                    oMail.Body = Replace(oMail.Body, "#data#", FormatDateTime(Now, DateFormat.LongDate))
                    oMail.Body = Replace(oMail.Body, "#comunita#", Me.Nome)

                    oMail.InviaMail()
                End If
            Catch ex As Exception

            End Try
        End Sub
        Public Sub MailAccettazione(ByVal oSubscriber As COL_Persona, ByVal Byuser As COL_Persona, ByVal oLocalizedMail As MailLocalized)
            Dim oResponsabile As New COL_Persona


            Try
                oResponsabile = Byuser
                oLocalizedMail = ManagerConfiguration.GetMailLocalized(oSubscriber.Lingua)
                '	oLocalizedMail.ReloadByLanguage(oPersona.Lingua.Codice)
                Dim oMail As New COL_E_Mail(oLocalizedMail)
                If oResponsabile.Errore = Errori_Db.None Then
                    oMail.IndirizziCC.Add(New MailAddress(oResponsabile.Mail, oResponsabile.Anagrafica))
                Else
                    Dim oCreatore As New COL_Persona
                    oCreatore = Me.GetCreatore
                    If oCreatore.Errore = Errori_Db.None Then
                        oMail.IndirizziCC.Add(New MailAddress(oCreatore.Mail, oCreatore.Anagrafica))
                    End If
                End If

                If oMail.IndirizziCC.Count > 0 Then
                    oMail.Mittente = oLocalizedMail.SystemSender
                    oMail.IndirizziTO.Add(New MailAddress(oSubscriber.Mail))

                    oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.CommunityAcceptSupscription).Subject
                    If Hour(Now) <= 12 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message
                    ElseIf Hour(Now) > 12 And Hour(Now) < 18 Then
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message
                    Else
                        oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message
                    End If

                    oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.CommunityAcceptSupscription).Message & vbCrLf & vbCrLf & vbCrLf
                    oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
                    oMail.Body = Replace(oMail.Body, "#data#", FormatDateTime(Now, DateFormat.LongDate))
                    oMail.Body = Replace(oMail.Body, "#comunita#", Me.Nome)

                    oMail.InviaMail()
                End If
            Catch ex As Exception

            End Try
        End Sub
        Public Shared Function Aggiungi_InAttesaConferma(ByVal SessionId As String, ByVal PRSN_ID As Integer, ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AggiungiInAttesaConferma"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
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
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        Public Function Rimuovi_InAttesaConferma(ByVal SessionId As String, ByVal PRSN_ID As Integer, ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RimuoviInAttesaConferma"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SessionId", SessionId, ParameterDirection.Input, DbType.String, False, 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@found", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(4) = 0 Then
                    Return False
                Else
                    Return True
                End If

            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

        Public Shared Sub RemoveWaitingSubscription(ByVal PersonID As Integer, ByVal CommunityID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RemoveWaitingSubscription"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CommunityID", CommunityID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PersonID", PersonID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
            End Try
        End Sub

#End Region

#Region "News For Comunita"
        'Public Shared Function News_Bacheca(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsBacheca"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_File(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean, Optional ByVal CTGR_ID As Integer = -1) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess


        '	With oRequest
        '		.Command = "sp_Comunita_NewsFile"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@CTGR_ID", CTGR_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_ReportCategorie_File(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsCategorieForFile"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_Forum(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsForum"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_Thread(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsThread"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_Post(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsPost"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Shared Function News_PostSummary(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsPostSummary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function

        'Public Shared Function News_MemoSummary(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsMemoSummary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function

        'Public Function News_IscrittiSummaryByRole(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsIscrittiSummaryByRole"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_IscrittiSummary(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsIscrittiSummary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_FileSummary(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess


        '	With oRequest
        '		.Command = "sp_Comunita_NewsFile_Summary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_ForumSummary(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest

        '		.Command = "sp_Comunita_NewsForumSummary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_ThreadSummary(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsThreadSummary"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_PostTotale(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsPostTotale"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function
        'Public Function News_EventiTotale(ByVal PersonaID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim oDataSet As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_NewsEventiTotale"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)

        '		If isLoggato Then
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 1, ParameterDirection.Input, SqlDbType.Int)
        '		Else
        '			oParam = objAccesso.GetAdvancedParameter("@isLoggato", 0, ParameterDirection.Input, SqlDbType.Int)
        '		End If
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		oDataSet = objAccesso.GetdataSet(oRequest)
        '	Catch ex As Exception

        '	End Try
        '	Return oDataSet
        'End Function

        'Public Shared Function News_GetFirstNewEvent(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DateTime
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim iResponse As Integer
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_GetFirstNewEvent"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)

        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)

        '		oParam = objAccesso.GetParameter("@DataFirstNewEvent", "", ParameterDirection.Output, DbType.DateTime)
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        '		Return oRequest.GetValueFromParameter(4)
        '	Catch ex As Exception
        '		Return Now
        '	End Try
        '	Return Now
        'End Function

        'Public Shared Function News_RaccoltaLink(ByVal CMNT_Id As Integer, ByVal PRSN_ID As Integer, ByVal isLoggato As Boolean) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim iResponse As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Comunita_News_RaccoltaLink"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_Id, , DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, , DbType.Int32)
        '		.Parameters.Add(oParam)

        '		If isLoggato Then
        '			oParam = objAccesso.GetParameter("@isLoggato", 1, , DbType.Int32)
        '		Else
        '			oParam = objAccesso.GetParameter("@isLoggato", 0, , DbType.Int32)
        '		End If
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		iResponse = objAccesso.GetdataSet(oRequest)
        '		Return iResponse
        '	Catch ex As Exception
        '		Return iResponse
        '	End Try
        '	Return iResponse
        'End Function

#End Region

        'Public Shared Function __GetAlberoComunitaCompleto(ByVal LinguaID As Integer, ByVal ORGN_ID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal TPCM_ID As Integer = -1, Optional ByVal oFiltro As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoCorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_GetAlberoCompleto"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltro, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@TipoCorsoDiStudi_Id", TipoCorsoDiStudi_Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function

        Public Shared Function ElencaIscrittiSottoComunita(ByVal ComunitaID As Integer, ByVal PRSN_Id As Integer, ByVal LinguaID As Integer, Optional ByVal ElencoRuoli_ID As String = "", Optional ByVal ElencoAttivazioni_ID As String = "", Optional ByVal pPAGE_SIZE As Integer = -1, Optional ByVal pPAGE_INDEX As Integer = -1, Optional ByVal Valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oCampoOrdine As FiltroCampoOrdine = FiltroCampoOrdine.anagrafica, Optional ByVal WithFoto As Boolean = False, Optional ByVal oFiltroUtente As FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaIscrittiSottoComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", ComunitaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ElencoRuoli_ID", ElencoRuoli_ID, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinamento", CType(oOrdinamento, FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinaPer", CType(oCampoOrdine, FiltroCampoOrdine), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ElencoAttivazioni_ID", ElencoAttivazioni_ID, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtente, FiltroUtenti), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If WithFoto Then
                    oParam = objAccesso.GetParameter("@WithFoto", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@WithFoto", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oTable
        End Function

#Region "Funzioni da ELIMINARE"
        'Public Shared Function _RicercaCorso(ByVal LinguaID As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroRicercaComunita As FiltroRicercaComunitaByIscrizione, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal oFiltro As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal oElencoRecord As Main.ElencoRecord = Main.ElencoRecord.Normale, Optional ByVal CompleteTree As Boolean = False, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_RicercaCorso_Paginate"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        'oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, , DbType.Int32)
        '        '.Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltro, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_INDEX", pageIndex, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_SIZE", pageSize, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@forAlbero", CType(oElencoRecord, Main.ElencoRecord), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        If CompleteTree Then
        '            oParam = objAccesso.GetParameter("@CompleteTree", 1, ParameterDirection.Input, DbType.Int32)
        '        Else
        '            oParam = objAccesso.GetParameter("@CompleteTree", 0, ParameterDirection.Input, DbType.Int32)
        '        End If
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroRicercaComunita", CType(oFiltroRicercaComunita, FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function
        Dim alfa As Integer
        'Public Shared Function RicercaCorso_FormatoLista(ByVal LinguaID As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroRicercaComunita As FiltroRicercaComunitaByIscrizione, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal oFiltro As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_RicercaCorso_FormatoLista"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CMNT_IdPadre", CMNT_IdPadre, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroComunita", CType(oFiltro, Main.FiltroComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_INDEX", pageIndex, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PAGE_SIZE", pageSize, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroRicercaComunita", CType(oFiltroRicercaComunita, FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function
#End Region

#Region "NUOVE funzioni ricerca"
        'Public Shared Function RicercaOrganizzazioniForIscrizione(ByVal LinguaID As Integer, ByVal PersonaID As Integer, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDs As DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Comunita_RicercaOrganizzazioniForIscrizione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroRicercaComunitaByIscrizione.nonIscritto, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDs = objAccesso.GetdataSet(oRequest)

        '    Catch ex As Exception

        '    End Try
        '    Return oDs
        'End Function
        'Public Shared Function RicercaCorsoForIscrizione(ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_RicercaCorsoForIscrizione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroRicercaComunitaByIscrizione.nonIscritto, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function

        'Public Shared Function RicercaTesiForIscrizione(ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal responsabileID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal SoloAttive As Boolean = True) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    Dim oData As String

        '    With oRequest
        '        .Command = "sp_Comunita_RicercaTesiForIscrizione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroRicercaComunitaByIscrizione.nonIscritto, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@responsabileID", responsabileID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        If SoloAttive Then
        '            oParam = objAccesso.GetAdvancedParameter("@SoloAttive", 1, ParameterDirection.Input, SqlDbType.Int)
        '        Else
        '            oParam = objAccesso.GetAdvancedParameter("@SoloAttive", 0, ParameterDirection.Input, SqlDbType.Int)
        '        End If
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oDataSet
        'End Function

        Public Shared Function RicercaResponsabili(ByVal FacoltaID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RicercaResponsabili"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
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
        Public Shared Function RicercaComunitaOrganizzazioni(ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione, ByVal LinguaID As Integer, ByVal PersonaID As Integer, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDs As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RicercaOrganizzazioniForIscrizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDs = objAccesso.GetdataSet(oRequest)

            Catch ex As Exception

            End Try
            Return oDs
        End Function
        Public Shared Function RicercaComunitaCorsi(ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione, ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RicercaCorsoForIscrizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
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
        Public Shared Function RicercaComunita(ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione, ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RicercaComunitaForIscrizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
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

        Public Shared Function RicercaComunitaForManagement(ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal Percorso As String = "", Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.forAdmin) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Comunita_RicercaComunitaForManagement"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Percorso", Percorso, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
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
        Public Shared Function RicercaComunitaAlberoForManagement(ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal Percorso As String = "", Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte, Optional ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.forAdmin, Optional ByVal ForAlberoAccesso As Boolean = False, Optional ByVal EscludiPath As Boolean = True) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_RicercaComunitaAlberoForManagement"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Percorso", Percorso, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ForAlberoAccesso Then
                    oParam = objAccesso.GetAdvancedParameter("@ForAlberoAccesso", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@ForAlberoAccesso", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)


                If EscludiPath Then
                    oParam = objAccesso.GetAdvancedParameter("@EscludiPath", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@EscludiPath", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
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
        Public Shared Function GeneraAlberoComunitaForAccesso(ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal Percorso As String = "", Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal oFiltroLetteraRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal CorsoDiStudi_Id As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Comunita_GeneraAlberoXML"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroTipoRicerca", CType(oFiltroTipoRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroLetteraRicerca", CType(oFiltroLetteraRicerca, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CorsoDiStudi_Id", CorsoDiStudi_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Percorso", Percorso, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
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

#Region "Gestione Esclusione Comunita"
        'Public Shared Function ElencaEsclusioneComunita(ByVal Principale_ID As Integer, ByVal PRSN_Id As Integer, ByVal LinguaID As Integer, Optional ByVal ORGN_ID As Integer = -1, Optional ByVal TipoCorsoDiStudi_Id As Integer = -1, Optional ByVal PeriodoID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal TPCM_ID As Integer = -1) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oTable As new DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Comunita_EscludiForIscrizione"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@CMNT_PrincipaleID", Principale_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PeriodoID", PeriodoID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TipoCorsoDiStudi_Id", TipoCorsoDiStudi_Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oTable = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return oTable
        'End Function

        Public Function ElencaComunitaEscluse(ByVal PRSN_Id As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Comunita_ElencaComunitaEscluse"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_PrincipaleID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oTable
        End Function

        Public Shared Function isUtenteEsclusoForIscrizione(ByVal Principale_ID As Integer, ByVal PRSN_Id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_isEsclusoForIscrizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_PrincipaleID", Principale_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                If oRequest.GetValueFromParameter(3) <> "" Then
                    If IsNumeric(oRequest.GetValueFromParameter(3)) Then
                        If oRequest.GetValueFromParameter(3) > 0 Then
                            Return True
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
#End Region

        Public Shared Function getMailAnagraficaIscritti(ByVal CMNT_ID As Integer, ByVal ElencoId As String) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_getMailAnagraficaIscritti"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ElencoId", ElencoId, ParameterDirection.Input, SqlDbType.Text)
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

        Public Function isBloccata() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_isBloccata"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@isBloccata", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(2) = "1" Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function isArchiviata() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_isArchiviata"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@isArchiviata", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(2) = "1" Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Shared Function BloccaMi(ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_BloccaMi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@blocca", 1, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True

            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Shared Function ArchiviaMi(ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ArchiviaMi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@archivia", 1, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Shared Function SbloccaMi(ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_BloccaMi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@blocca", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True

            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Shared Function DeArchiviaMi(ByVal CMNT_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ArchiviaMi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@archivia", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Shared Function GetTipolgia(ByVal CMNT_ID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = -1
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_GetTipolgia"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPCM_ID", -1, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Return CInt(oRequest.GetValueFromParameter(2))

            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function

#Region "Gestione Cover"
        Public Function ShowCover(ByVal PersonaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasCover"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@HasCover", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return (oRequest.GetValueFromParameter(2) = 1)

            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Shared Function ShowCover(ByVal ComunitaID As Integer, ByVal PersonaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_HasCover"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@HasCover", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return (oRequest.GetValueFromParameter(2) = 1)

            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Shared Function ElencoPagineDefault(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencoPagineDefault"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                Return oDataset
            Catch ex As Exception

            End Try
            Return oDataset
        End Function


        Public Shared Function GetDefaultPage(ByVal CMNT_ID As Integer, ByRef Percorso As String, ByRef Codice As String, ByRef Idpagina As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_GetDefaultPage"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@defaultPage", "", ParameterDirection.Output, SqlDbType.VarChar, True, 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Codice", "", ParameterDirection.Output, SqlDbType.VarChar, True, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Idpagina", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Percorso = oRequest.GetValueFromParameter(2)
                Codice = oRequest.GetValueFromParameter(3)
                Idpagina = oRequest.GetValueFromParameter(4)
                iResponse = True
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
        Public Shared Function SetDefaultPage(ByVal CMNT_ID As Integer, ByRef DFLP_ID As String) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Boolean = False

            With oRequest
                .Command = "sp_Comunita_SetDefaultPage"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DFLP_ID", DFLP_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = True
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
#End Region

        Public Shared Function GetStatusByOrganizzazione(ByVal ORGN_ID As Integer, ByRef totale As Integer, ByRef totaleArchiviate As Integer, ByRef totaleBloccate As Integer, Optional ByVal TipoComunitaId As Integer = -1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_GetStatusByOrganizzazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function RuoliAssociabiliByPersonaCol(ByVal PRSN_Id As Integer, ByVal oFiltro As FiltroRuoli, Optional ByVal oFiltroUtenti As FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As GenericCollection(Of Role)
            'elenca in una generic collection i ruoli che sono assegnati ad una comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliAssociabiliByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(oFiltroUtenti, FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim oListRuoli As New GenericCollection(Of Role)

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                While oDataReader.Read
                    oListRuoli.Add(New Role(CInt(oDataReader("TPRL_id")), CStr(oDataReader("TPRL_nome")), CInt(oDataReader("TPRL_Gerarchia"))))
                End While
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            Finally
                If oDataReader.IsClosed = False Then
                    oDataReader.Close()
                End If
            End Try
            Return oListRuoli
        End Function

#Region "Lazy Iscritti comunita"


        'Recupero l'elenco di tutti gli iscritti ad una comunita dal dB
        Public Shared Function GetAllIscritti(ByVal IdComunita As Integer, ByVal IdLingua As Integer) As List(Of Iscritto) 'As GenericCollection(Of Iscritto)

            'elenca in una generic collection TUTTI gli iscritti indiscriminatamente, senza nessun filtro

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim oListIscritti As New List(Of Iscritto) 'New GenericCollection(Of Iscritto)

            With oRequest
                .Command = "NEW_sp_ComunitaIscritti_2"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", IdComunita, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", IdLingua, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With



            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read

                    Dim oIscritto As New Iscritto
                    Dim id As Integer

                    Try

                        ' ''genericvalidator
                        id = oDataReader("RLPC_id")
                        Dim oPersona As New COL_Persona(oDataReader("PRSN_id"), oDataReader("PRSN_Nome"), oDataReader("PRSN_Cognome")) 'OK
                        oPersona.Cellulare = GenericValidator.ValString(oDataReader("PRSN_Cellulare"), "") 'OK

                        oPersona.DataNascita = GenericValidator.ValData((oDataReader("PRSN_DataNascita")), DataNull) 'OK
                        oPersona.Login = oDataReader("PRSN_Login") 'OK
                        oPersona.Mail = oDataReader("PRSN_Mail")   'OK

                        oPersona.MostraMail = oDataReader("PRSN_mostraMail") 'OK
                        ''''GenericValidator.ValBool((oDataReader("PRSN_mostraMail")), False)
                        oIscritto.RLPC_PRSN_mostraMail = oDataReader("RLPC_PRSN_mostraMail") 'OK

                        'If IsDBNull(oDataReader("PRSN_fotoPath")) Then
                        '    oPersona.FotoPath = ""
                        'Else
                        oPersona.FotoPath = GenericValidator.ValString(oDataReader("PRSN_fotoPath"), "")
                        'End If


                        oIscritto.Persona = oPersona 'OK

                        oIscritto.PRSN_TPPR_id = oDataReader("PRSN_TPPR_id")
                        'oIscritto.LKPO_Default = GenericValidator.ValInteger(oDataReader("LKPO_Default"), -100)

                        oIscritto.RLPC_abilitato = CBool(oDataReader("RLPC_abilitato"))
                        oIscritto.RLPC_attivato = CBool(oDataReader("RLPC_attivato"))
                        oIscritto.RLPC_id = oDataReader("RLPC_id")
                        oIscritto.RLPC_IscrittoIl = GenericValidator.ValData(oDataReader("RLPC_IscrittoIl"), DataNull)

                        oIscritto.RLPC_Responsabile = GenericValidator.ValBool(oDataReader("RLPC_Responsabile"), False)

                        oIscritto.RLPC_ultimoCollegamento = GenericValidator.ValData(oDataReader("RLPC_ultimoCollegamento"), DataNull)
                        oIscritto.STDN_matricola = GenericValidator.ValString(oDataReader("STDN_matricola"), "") 'Solo Studenti hanno matricola!!!

                        Dim oTipoRuolo As New Role(oDataReader("TPRL_id"), GenericValidator.ValString(oDataReader("TPPR_descrizione"), ""), oDataReader("TPRL_gerarchia"), Not oDataReader("TPRL_noDelete"), Not oDataReader("TPRL_noModify"))
                        oTipoRuolo.Name = oDataReader("TPRL_Nome")
                        oIscritto.Ruolo = oTipoRuolo

                        oListIscritti.Add(oIscritto)

                    Catch ex As Exception

                    End Try
                End While
            Catch ex As Exception
                'Me.n_Errore = Errori_Db.DBError
            Finally
                Try
                    If oDataReader.IsClosed = False Then
                        oDataReader.Close()
                    End If
                Catch ex As Exception
                End Try
            End Try

            Return oListIscritti
        End Function

        ''' <summary>
        '''    Recupera un elenco di iscritti con i soli dati di interesse per l'elenco mail.
        ''' </summary>
        ''' <param name="IdComunita">STRINGA: id delle comunità separati va virgola. Devono cominciare a finire per la stessa, ad esempio ,1, o ,1,2,146,</param>
        ''' <param name="IdLingua">ID della lingua per l'internazionalizzazione... (non serve, solo per compatibilità e sviluppi futuri)</param>
        ''' <param name="Nome">Per filtrare la ricerca</param>
        ''' <param name="Cognome">Per filtrare la ricerca</param>
        ''' <param name="Matricola">Per filtrare la ricerca</param>
        ''' <param name="Mail">Per filtrare la ricerca</param>
        ''' <param name="Login">Per filtrare la ricerca</param>
        ''' <returns>Lista di oggetti Iscritto</returns>
        ''' <remarks>La ricerca viene effettuata da dB, poichè il collo di bottiglia sta nel passaggio di dati tra dB e Server Web</remarks>
        Public Shared Function GetAllIscrittiComunitaMultipleFiltrate(
                ByVal IdComunita As String,
                ByVal IdLingua As Integer,
                Optional ByVal Nome As String = "",
                Optional ByVal Cognome As String = "",
                Optional ByVal Matricola As String = "",
                Optional ByVal Mail As String = "",
                Optional ByVal Login As String = "",
                Optional ByVal IdTipo As Integer = -1
                ) As List(Of Iscritto) 'As GenericCollection(Of Iscritto)

            'elenca in una generic collection TUTTI gli iscritti indiscriminatamente, senza nessun filtro

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim oListIscritti As New List(Of Iscritto) 'New GenericCollection(Of Iscritto)

            With oRequest
                .Command = "NEW_sp_ComunitaMultipleIscrittiFiltered"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", IdComunita, ParameterDirection.Input, SqlDbType.VarChar, False, 800)
                .Parameters.Add(oParam)

                'oParam = objAccesso.GetAdvancedParameter("@LinguaID", IdLingua, ParameterDirection.Input, SqlDbType.Int)
                '.Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Nome", Nome, ParameterDirection.Input, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Cognome", Cognome, ParameterDirection.Input, SqlDbType.VarChar, False, 40)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Matricola", Matricola, ParameterDirection.Input, SqlDbType.VarChar, False, 10)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Mail", Mail, ParameterDirection.Input, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Login", Login, ParameterDirection.Input, SqlDbType.VarChar, False, 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@IdTipo", IdTipo, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)

                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read

                    Dim oIscritto As New Iscritto

                    Try

                        ' ''genericvalidator
                        'id = oDataReader("RLPC_id")
                        Dim oPersona As New COL_Persona(oDataReader("PRSN_id"), oDataReader("PRSN_Nome"), oDataReader("PRSN_Cognome")) 'OK
                        'oPersona.Cellulare = GenericValidator.ValString(oDataReader("PRSN_Cellulare"), "") 'OK

                        'oPersona.DataNascita = GenericValidator.ValData((oDataReader("PRSN_DataNascita")), DataNull) 'OK
                        oPersona.Login = oDataReader("PRSN_Login") 'OK
                        oPersona.Mail = oDataReader("PRSN_Mail")   'OK

                        oPersona.MostraMail = oDataReader("PRSN_mostraMail") 'OK
                        'oIscritto.RLPC_PRSN_mostraMail = oDataReader("RLPC_PRSN_mostraMail") 'OK

                        'oPersona.FotoPath = GenericValidator.ValString(oDataReader("PRSN_fotoPath"), "")
                        oIscritto.Persona = oPersona 'OK

                        oIscritto.PRSN_TPPR_id = oDataReader("PRSN_TPPR_id")
                        'oIscritto.RLPC_abilitato = CBool(oDataReader("RLPC_abilitato"))
                        'oIscritto.RLPC_attivato = CBool(oDataReader("RLPC_attivato"))
                        'oIscritto.RLPC_id = oDataReader("RLPC_id")
                        'oIscritto.RLPC_IscrittoIl = GenericValidator.ValData(oDataReader("RLPC_IscrittoIl"), DataNull)

                        'oIscritto.RLPC_Responsabile = GenericValidator.ValBool(oDataReader("RLPC_Responsabile"), False)

                        'oIscritto.RLPC_ultimoCollegamento = GenericValidator.ValData(oDataReader("RLPC_ultimoCollegamento"), DataNull)
                        oIscritto.STDN_matricola = GenericValidator.ValString(oDataReader("STDN_matricola"), "") 'Solo Studenti hanno matricola!!!

                        'Dim oTipoRuolo As New TipoRuolo(oDataReader("TPRL_id"), GenericValidator.ValString(oDataReader("TPPR_descrizione"), ""), oDataReader("TPRL_gerarchia"), oDataReader("TPRL_noDelete"), oDataReader("TPRL_noModify"))
                        'oTipoRuolo.Nome = oDataReader("TPRL_Nome")
                        'oIscritto.Ruolo = oTipoRuolo

                        oListIscritti.Add(oIscritto)

                    Catch ex As Exception

                    End Try
                End While
            Catch ex As Exception
                'Me.n_Errore = Errori_Db.DBError
            Finally
                Try
                    If oDataReader.IsClosed = False Then
                        oDataReader.Close()
                    End If
                Catch ex As Exception
                End Try
            End Try

            Return oListIscritti
        End Function

        ''Recupero tutti i ruoli di una determinata lingua
        ''Se non ci sono in cache, li recupero da dB e li aggiungo alla cache
        'Public Shared Function GetTipoRuoloLazy(ByVal IdLingua As Integer) As Hashtable 'As TipoRuolo - ByVal IdTipoRuolo As Integer,
        '    Dim oTipiRuoli As New Hashtable  'New GenericCollection(Of TipoRuolo)
        '    Dim cacheKey As String = "TipiRuolo_" & IdLingua
        '    If ObjectBase.Cache(cacheKey) Is Nothing Then
        '        'Se non c'e', carico la lista
        '        Dim oRequest As New COL_Request
        '        Dim oParam As New COL_Request.Parameter
        '        Dim oDataReader As IDataReader
        '        Dim objAccesso As New COL_DataAccess

        '        With oRequest
        '            .Command = "NEW_sp_TipoRuolo_Elenco"
        '            .CommandType = CommandType.StoredProcedure
        '            oParam = objAccesso.GetAdvancedParameter("@LinguaID", IdLingua, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)
        '            .Role = COL_Request.UserRole.Admin
        '            .transactional = False
        '            Try
        '                oDataReader = objAccesso.GetdataReader(oRequest)
        '                While oDataReader.Read
        '                    Dim oTipoRuolo As New TipoRuolo
        '                    oTipoRuolo.Id = oDataReader("TPRL_id")

        '                    oTipoRuolo.Nome = CStr(oDataReader("TPRL_nome"))

        '                    Try
        '                        oTipoRuolo.Descrizione = CStr(oDataReader("TPPR_descrizione"))
        '                    Catch ex As Exception
        '                        oTipoRuolo.Descrizione = ""
        '                    End Try

        '                    oTipoRuolo.Gerarchia = CInt(oDataReader("TPRL_gerarchia"))
        '                    oTipoRuolo.NoDelete = CBool(oDataReader("TPRL_noDelete"))
        '                    oTipoRuolo.NoModify = CBool(oDataReader("TPRL_noModify"))
        '                    oTipiRuoli.Add(oTipoRuolo.Id, oTipoRuolo)
        '                End While
        '            Catch ex As Exception

        '            Finally
        '                If oDataReader.IsClosed = False Then
        '                    oDataReader.Close()
        '                End If
        '            End Try

        '            'e l'aggiungo alla cache
        '            ObjectBase.Cache.Insert(cacheKey, oTipiRuoli, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))

        '        End With
        '    Else
        '        oTipiRuoli = CType(ObjectBase.Cache(cacheKey), Hashtable)
        '    End If

        '    'Return oTipiRuoli.Find(New GenericPredicate(Of TipoRuolo, Integer)(IdTipoRuolo, AddressOf FindByTipoRuoloID))
        '    Return oTipiRuoli '(IdTipoRuolo)
        'End Function
        Private Shared Function FindByTipoRuoloID(ByVal item As Role, ByVal argument As Integer) As Boolean
            Return item.ID = argument
        End Function

        'Applico i filtri all'elenco iscritti di una comunita. Se l'elenco non è in cache lo recupero dal dB.
        'Non ancora paginati!!! (III funzione?!=
        Public Shared Function GetIscrittiLazy(
         ByRef Totale As Integer,
         ByVal ComunitaID As Integer,
         ByVal LinguaID As Integer,
         ByVal IdPersona As Integer,
         ByVal oFiltroAbilitazione As FiltroAbilitazione,
         ByVal oFiltroUtente As FiltroUtenti,
         Optional ByVal TPRL_id As Integer = -1,
         Optional ByVal Valore As String = "",
         Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti,
         Optional ByVal WithFoto As Boolean = False,
         Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti) _
         As List(Of Iscritto)

            Dim oIscritti As New List(Of Iscritto)
            Dim cacheKey As String = CachePolicy.ListaIScritti(ComunitaID, LinguaID)

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oIscritti = GetAllIscritti(ComunitaID, LinguaID)
                ObjectBase.Cache.Insert(cacheKey, oIscritti, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
            Else
                oIscritti = CType(ObjectBase.Cache(cacheKey), List(Of Iscritto)) 'GenericCollection(Of Iscritto))
            End If


            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, Boolean)(True, AddressOf FiltroGenerale))
            'oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, Integer)(IdPersona, AddressOf IscrittiFiltraIdPersona))


            If Not oFiltroAbilitazione = FiltroAbilitazione.Tutti Then
                oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, FiltroAbilitazione)(oFiltroAbilitazione, AddressOf IscrittiFiltraAbilitazione))
            End If

            If Not oFiltroUtente = FiltroUtenti.Tutti Then
                oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, FiltroUtenti)(oFiltroUtente, AddressOf IscrittiFiltraFiltroUtente))
            End If

            If Not TPRL_id = -1 Then
                oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, Integer)(TPRL_id, AddressOf IscrittiFiltraTPRL_id))
            End If

            If WithFoto Then
                oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, Boolean)(WithFoto, AddressOf IscrittiFiltraWithFoto))
            End If

            If Not oAnagrafica = FiltroAnagrafica.tutti Then
                oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, FiltroAnagrafica)(oAnagrafica, AddressOf IscrittiFiltraAnagrafica))
            End If

            If Not Valore = "" Then
                If Not oFiltroRicerca = FiltroRicercaAnagrafica.tutti Then
                    Select Case oFiltroRicerca
                        Case FiltroRicercaAnagrafica.nome
                            'Valore
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraNome))
                        Case FiltroRicercaAnagrafica.cognome
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraCognome))
                        Case FiltroRicercaAnagrafica.nomeCognome
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraNomeCognome))
                        Case FiltroRicercaAnagrafica.matricola
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraMatricola))
                        Case FiltroRicercaAnagrafica.login
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraLogin))
                        Case FiltroRicercaAnagrafica.dataNascita
                            oIscritti = oIscritti.FindAll(New GenericPredicate(Of Iscritto, String)(Valore, AddressOf IscrittiFiltraDataNascita))
                    End Select
                End If
            End If
            Totale = oIscritti.Count
            Return oIscritti
        End Function


#Region "Filtri per iscrizioni"
        Private Shared Function FiltroGenerale(ByVal item As Iscritto, ByVal argument As Boolean) As Boolean
            If (item.Ruolo.ID = -2) Or (item.Ruolo.ID = -3) Or (item.Ruolo.ID = -4) Then
                Return False
            Else
                Return True
            End If
        End Function
        Private Shared Function IscrittiFiltraIdPersona(ByVal item As Iscritto, ByVal argument As Integer) As Boolean
            Return item.Persona.ID <> argument
        End Function
        Private Shared Function IscrittiFiltraAbilitazione(ByVal item As Iscritto, ByVal argument As FiltroAbilitazione) As Boolean
            Select Case argument
                Case FiltroAbilitazione.Abilitato '0
                    Return True
                Case FiltroAbilitazione.AttivatoAbilitato '1
                    Return item.RLPC_abilitato = True And item.RLPC_attivato = True
                    ' (RLPC_Abilitato = 1) AND  (RLPC_Attivato = 1)  '
                Case FiltroAbilitazione.Attivato '2
                    Return item.RLPC_attivato = True
                    '(RLPC_Attivato = 1)  '
                Case FiltroAbilitazione.Abilitato '3
                    Return item.RLPC_abilitato = True
                    ' and (RLPC_Abilitato = 1) '
                Case FiltroAbilitazione.NonAttivato '4
                    Return item.RLPC_attivato = False
                    '  AND  (RLPC_Attivato = 0) '
                Case FiltroAbilitazione.NonAbilitato '5
                    Return item.RLPC_abilitato = False
                    ' AND (RLPC_Abilitato = 0) '
                Case FiltroAbilitazione.NonAttivatoNonAbilitato '6
                    Return item.RLPC_abilitato = False And item.RLPC_attivato = False
                    ' AND (RLPC_Abilitato = 0) AND  (RLPC_Attivato = 0)  '
                Case FiltroAbilitazione.NonAbilitatoAttivato '7
                    Return item.RLPC_abilitato = True And item.RLPC_attivato = False
                    '  AND (RLPC_Abilitato = 0) AND  (RLPC_Attivato = 1) '
                Case FiltroAbilitazione.TuttiUltimiIscritti '8
                    'RIVEDERE!!!!
                    'WHERE     (Amministratore.RLPC_CMNT_id =  @CMNT_ID as varchar) + 
                    'AND Nuovi.RLPC_CMNT_id =  ' + cast(@CMNT_ID as varchar) + ') AND (Amministratore.RLPC_PRSN_id = ' + cast(@PRSN_ID as varchar) + ')'
                    '				set @SQL_condition = @SQL_condition + ') '
                    '			end
                Case FiltroAbilitazione.IscrizioneNonConfermata
                    '?!?!? NON IN STORE!!!!
                    Return True
                Case Else
                    Return True
            End Select
        End Function
        Private Shared Function IscrittiFiltraFiltroUtente(ByVal item As Iscritto, ByVal argument As FiltroUtenti) As Boolean
            Select Case argument
                Case FiltroUtenti.NoPassanti
                    Return item.Ruolo.ID <> -3
                    '  AND  TPRL_id  <> -3  ' 
                Case FiltroUtenti.NoCreatori
                    Return item.Ruolo.ID <> -2
                    '  AND  TPRL_id  <> -2  ' 
                Case FiltroUtenti.NoPassantiNoCreatori
                    Return (item.Ruolo.ID <> -3) And (item.Ruolo.ID <> -2)
                    '  AND  TPRL_id  <> -3 AND  TPRL_id  <> -2  ' 
                Case FiltroUtenti.Passanti
                    Return item.Ruolo.ID = -3
                    '  AND  TPRL_id   = -3 ' 
                Case FiltroUtenti.Creatori
                    Return item.Ruolo.ID <> -2
                    '  AND  TPRL_id  = -2  ' 
                Case FiltroUtenti.PassantiCreatori
                    Return (item.Ruolo.ID = -3) And (item.Ruolo.ID = -2)
                    '  AND  TPRL_id  = -3 AND  TPRL_id  = -2  ' 
                Case FiltroUtenti.Responsabili
                    Return item.Ruolo.ID = 1 'Amministratore comunita
                Case Else
                    Return True
            End Select

        End Function
        Private Shared Function IscrittiFiltraTPRL_id(ByVal item As Iscritto, ByVal argument As Integer) As Boolean
            Return item.Ruolo.ID = argument
        End Function
        Private Shared Function IscrittiFiltraWithFoto(ByVal item As Iscritto, ByVal argument As Boolean) As Boolean
            If IsNothing(item.Persona.FotoPath) Then
                Return True
            Else
                If item.Persona.FotoPath <> "" Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function
        Private Shared Function IscrittiFiltraAnagrafica(ByVal item As Iscritto, ByVal argument As FiltroAnagrafica) As Boolean
            If argument = FiltroAnagrafica.altro Then
                Dim carattere1, carattere2 As Integer
                carattere1 = CInt(argument) + 64
                carattere2 = CInt(argument) + 96
                ' A = 41Hex = 65
                ' Z = 5AHex = 90 (25)
                ' a = 61Hex = 97
                ' z = 7AHex = 122 (25)
                If Not item.Persona.Cognome.StartsWith(Chr(carattere1).ToString) Or item.Persona.Cognome.StartsWith(Chr(carattere2).ToString) Then
                    Return True
                End If
            ElseIf argument >= FiltroAnagrafica.a And argument <= FiltroAnagrafica.z Then
                Dim carattere1, carattere2 As Integer
                carattere1 = CInt(argument) + 64
                carattere2 = CInt(argument) + 96
                ' A = 41Hex = 65
                ' Z = 5AHex = 90 (25)
                ' a = 61Hex = 97
                ' z = 7AHex = 122 (25)

                'If item.Persona.Cognome.Contains(Chr(carattere1)) Or item.Persona.Cognome.Contains(Chr(carattere2)) Then
                If item.Persona.Cognome.StartsWith(Chr(carattere1).ToString) Or item.Persona.Cognome.StartsWith(Chr(carattere2).ToString) Then
                    Return True
                End If
            End If

        End Function
        Public Shared Function GetIscrittoLazyByPersona(ByVal IdPersona As Integer, ByVal ComunitaId As Integer, ByVal LinguaId As Integer) As Iscritto

            Dim oIscritto As Iscritto

            Dim oIscritti As New List(Of Iscritto)
            Dim cacheKey As String = CachePolicy.ListaIScritti(ComunitaId, LinguaId)

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oIscritti = GetAllIscritti(ComunitaId, LinguaId)
                ObjectBase.Cache.Insert(cacheKey, oIscritti, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
            Else
                oIscritti = CType(ObjectBase.Cache(cacheKey), List(Of Iscritto)) 'GenericCollection(Of Iscritto))
            End If

            oIscritto = oIscritti.Find(New GenericPredicate(Of Iscritto, Integer)(IdPersona, AddressOf FindByPersonaID))

            Return oIscritto

        End Function

        Public Shared Function GetIscrittoLazyByIscritto(ByVal IdIscritto As Integer, ByVal ComunitaId As Integer, ByVal LinguaId As Integer) As Iscritto

            Dim oIscritto As Iscritto

            Dim oIscritti As New List(Of Iscritto)
            Dim cacheKey As String = CachePolicy.ListaIScritti(ComunitaId, LinguaId)

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oIscritti = GetAllIscritti(ComunitaId, LinguaId)
                ObjectBase.Cache.Insert(cacheKey, oIscritti, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
            Else
                oIscritti = CType(ObjectBase.Cache(cacheKey), List(Of Iscritto)) 'GenericCollection(Of Iscritto))
            End If

            oIscritto = oIscritti.Find(New GenericPredicate(Of Iscritto, Integer)(IdIscritto, AddressOf FindByIscrittoID))

            Return oIscritto

        End Function

        Private Shared Function IscrittiFiltraNome(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.Persona.Nome.Contains(argument)
        End Function
        Private Shared Function IscrittiFiltraCognome(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.Persona.Cognome.Contains(argument)
        End Function
        Private Shared Function IscrittiFiltraNomeCognome(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.Persona.Anagrafica.Contains(argument)
        End Function
        Private Shared Function IscrittiFiltraMatricola(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.STDN_matricola.Contains(argument)
        End Function
        Private Shared Function IscrittiFiltraLogin(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.Persona.Login.Contains(argument)
        End Function
        Private Shared Function IscrittiFiltraDataNascita(ByVal item As Iscritto, ByVal argument As String) As Boolean
            Return item.Persona.DataNascita.ToString.Contains(argument)
        End Function

        Private Shared Function FindByPersonaID(ByVal item As Iscritto, ByVal argument As Integer) As Boolean
            Return item.Persona.ID = argument
        End Function
        Private Shared Function FindByIscrittoID(ByVal item As Iscritto, ByVal argument As Integer) As Boolean
            Return item.RLPC_id = argument
        End Function
#End Region

        'Public Shared Function ElencaListaID_ForServizioList(ByVal LinguaID As Integer, ByVal SRVZ_Codice As String, ByVal PRSN_Id As Integer, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As List(Of COL_Comunita)

        '    Dim Serv_ElComList As New List(Of COL_Comunita)

        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataReader As IDataReader
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Comunita_ElencaListaSemplice_ForServizio"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@SRVZ_Codice", SRVZ_Codice, ParameterDirection.Input, SqlDbType.VarChar, , 32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDataReader = objAccesso.GetdataReader(oRequest)
        '        Dim DataNull As New DateTime(2000, 1, 1)

        '        While oDataReader.Read

        '            'Dim oServCom As New UCServices.Services_ElencaComunita
        '            Dim oComunita As New COL_Comunita
        '            Dim insert As Boolean = False
        '            Dim id As Integer
        '            'SELECT  ,,,  , ,  ,,,,,

        '            Try
        '                With oComunita
        '                    Dim servicepermessi As New UCServices.Services_Mail(oDataReader("LKSC_Permessi"))

        '                    If servicepermessi.Admin Or servicepermessi.SendMail Then
        '                        insert = True
        '                        'Se ci dovessero essere di NULL da DB utilizzare i generic validator!!!
        '                        .Id = oDataReader("CMNT_ID")
        '                        '.IdPadre = oDataReader("CMNT_idPadre")
        '                        '.CreatoreID = oDataReader("CMNT_PRSN_Id")
        '                        '.Organizzazione.Id = oDataReader("CMNT_ORGN_id")
        '                        '.Organizzazione.Estrai()

        '                        '.TipoComunita.ID = oDataReader("CMNT_TPCM_id")
        '                        '.TipoComunita.Estrai()
        '                        .Nome = oDataReader("CMNT_nome")
        '                        '.IsChiusa = GenericValidator.ValBool(oDataReader("CMNT_IsChiusa "), True)
        '                        'oDataReader("CMNT_PRFS_ID") 'e ke è sta robba?!?! E' sempre zero... :s
        '                        .HasAccessoLibero = GenericValidator.ValBool(oDataReader("CMNT_AccessoLibero"), False)
        '                        '.HasAccessoCopisteria = GenericValidator.ValBool(oDataReader("CMNT_AccessoCopisteria"), False)
        '                    Else
        '                        insert = False
        '                    End If
        '                End With

        '                'oPersona.FotoPath = GenericValidator.ValString(oDataReader("PRSN_fotoPath"), "")
        '                'oIscritto.RLPC_abilitato = CBool(oDataReader("RLPC_abilitato"))
        '                'oIscritto.RLPC_IscrittoIl = GenericValidator.ValData(oDataReader("RLPC_IscrittoIl"), DataNull)
        '                'oIscritto.RLPC_Responsabile = GenericValidator.ValBool(oDataReader("RLPC_Responsabile"), False)
        '                'oIscritto.STDN_matricola = GenericValidator.ValString(oDataReader("STDN_matricola"), "") 'Solo Studenti hanno matricola!!!

        '                If insert Then
        '                    Serv_ElComList.Add(oComunita)
        '                End If

        '            Catch ex As Exception

        '            End Try
        '        End While
        '    Catch ex As Exception
        '        'Me.n_Errore = Errori_Db.DBError
        '    Finally
        '        Try
        '            If oDataReader.IsClosed = False Then
        '                oDataReader.Close()
        '            End If
        '        Catch ex As Exception
        '        End Try
        '    End Try

        '    Return Serv_ElComList
        'End Function

        Public Shared Function ElencaListaID_ForServizio(ByVal LinguaID As Integer, ByVal SRVZ_Codice As String, ByVal PRSN_Id As Integer, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As String
            Dim ListaId As String = ","
            Dim Serv_ElComList As New List(Of COL_Comunita)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_ElencaListaSemplice_ForServizio"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Codice", SRVZ_Codice, ParameterDirection.Input, SqlDbType.VarChar, , 32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read

                    Try

                        Dim servicepermessi As New UCServices.Services_Mail(oDataReader("LKSC_Permessi"))

                        If servicepermessi.Admin Or servicepermessi.SendMail Then
                            'Se ci dovessero essere di NULL da DB utilizzare i generic validator!!!
                            ListaId &= oDataReader("CMNT_ID") & ","
                        End If
                    Catch ex As Exception

                    End Try
                End While
            Catch ex As Exception
                'Me.n_Errore = Errori_Db.DBError
            Finally
                Try
                    If oDataReader.IsClosed = False Then
                        oDataReader.Close()
                    End If
                Catch ex As Exception
                End Try
            End Try

            Return ListaId 'Serv_ElComList
        End Function

#End Region

        Public Shared Function GetAccessoLibero(ByVal ComunitaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Boolean = False

            With oRequest
                .Command = "sp_Comunita_GetAccessoLibero"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@isLibero", False, ParameterDirection.Output, SqlDbType.Bit)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = CBool(oRequest.GetValueFromParameter(2))
            Catch ex As Exception
                iResponse = False
            End Try

            Return iResponse
        End Function


        Public Shared Function LazyGetDefaultPage(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As ServicePage
            Dim oLista As New List(Of ServicePage)
            Dim oPage As ServicePage
            Dim cacheKey As String = CachePolicy.PagineDefault(ComunitaID, LinguaID)


            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oLista = COL_Comunita.RetrieveDBDefaultService(ComunitaID, LinguaID)
                ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
            Else
                oLista = CType(ObjectBase.Cache(cacheKey), List(Of ServicePage))
            End If
            oPage = oLista.Find(New GenericPredicate(Of ServicePage, Boolean)(True, AddressOf ServicePage.FindByDefault))
            Return oPage
        End Function
        Public Shared Function LazyGetCurrentDefaultPage(ByVal ComunitaID As Integer) As ServicePage
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oPage As ServicePage
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_GetDefaultPage"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@defaultPage", "", ParameterDirection.Output, SqlDbType.VarChar, True, 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Codice", "", ParameterDirection.Output, SqlDbType.VarChar, True, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Idpagina", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                oPage = New ServicePage
                oPage.Url = oRequest.GetValueFromParameter(2)
                oPage.ID = oRequest.GetValueFromParameter(4)
                oPage.Servizio = New PlainServiceComunita
                oPage.Servizio.Code = oRequest.GetValueFromParameter(3)
                'Codice = oRequest.GetValueFromParameter(3)
                '	Idpagina = oRequest.GetValueFromParameter(4)
                Return oPage
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Public Shared Function LazyListOfDefaultPage(ByVal ComunitaID As Integer, ByVal LinguaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of ServicePage)
            Dim oLista As New List(Of ServicePage)
            Dim cacheKey As String = CachePolicy.PagineDefault(ComunitaID, LinguaID)

            If sortDirection <> String.Empty Then
                sortDirection = sortDirection.ToLower
            End If

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oLista = COL_Comunita.RetrieveDBDefaultService(ComunitaID, LinguaID)
                ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
            Else
                oLista = CType(ObjectBase.Cache(cacheKey), List(Of ServicePage))
            End If

            If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
                oLista.Reverse()
            End If

            Return oLista
        End Function
        Private Shared Function RetrieveDBDefaultService(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As List(Of ServicePage)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess
            Dim iLista As New List(Of ServicePage)

            With oRequest
                .Command = "sp_Comunita_ElencoPagineDefault"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iDataReader = objAccesso.GetdataReader(oRequest)

                Try
                    While iDataReader.Read
                        iLista.Add(New ServicePage(LinguaID, iDataReader.Item("DFLP_ID"), GenericValidator.ValString(iDataReader.Item("DFLP_Nome"), ""),
                        iDataReader.Item("DFLP_Pagina"), iDataReader.Item("DFLP_Default"), ComunitaID,
                        New PlainServiceComunita(iDataReader.Item("DFLP_SRVZ_ID"), iDataReader.Item("SRVZ_Codice"), iDataReader.Item("SRVZ_Attivato"), iDataReader.Item("SRVC_isAbilitato"), ComunitaID)))
                    End While
                Catch ex As Exception

                Finally
                    If Not iDataReader.IsClosed Then
                        iDataReader.Close()
                    End If
                End Try
            Catch ex As Exception

            End Try
            Return iLista
        End Function

        'Public Shared Function FindIscrizioneComunita(ByVal CurrentUser As COL_Persona, ByVal LinguaID As Integer, ByVal ComunitaId As Integer) As IscrizioneComunita
        '    Dim oIscrizioni As GenericCollection(Of IscrizioneComunita)
        '    Dim cacheKey As String = "PlainIscrizioni_" & CurrentUser.Id & "_" & LinguaID

        '    If ObjectBase.Cache(cacheKey) Is Nothing Then
        '        oIscrizioni = IscrizioneComunita.PlainIscrizioni(CurrentUser, LinguaID)
        '        oIscrizioni.Sort(New GenericComparer(Of IscrizioneComunita)("IscrittoIl"))
        '        ObjectBase.Cache.Insert(cacheKey, oIscrizioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
        '    Else
        '        oIscrizioni = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of IscrizioneComunita))
        '    End If
        '    Return oIscrizioni.Find(New GenericPredicate(Of IscrizioneComunita, Integer)(ComunitaId, AddressOf FindByCommunityID))
        'End Function

        'Private Shared Function FindByCommunityID(ByVal item As IscrizioneComunita, ByVal argument As Integer) As Boolean
        '    Return item._Comunita.Id = argument
        'End Function

        Public Function isEqual(ByVal other As COL_Comunita) As Boolean Implements System.IEquatable(Of COL_Comunita).Equals
            If Me.n_CMNT_id = other.Id Then
                Return True
            Else
                Return False
            End If
        End Function


        Public Shared Function FindCommunity(ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione, ByVal LinguaID As Integer, ByVal FacoltàID As Integer, Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal CommunityName As String = "", Optional ByVal ResponsabileID As Integer = -2, Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_FindCommunityForSubscription"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", CommunityName, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltàID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ResponsabileID", ResponsabileID, ParameterDirection.Input, SqlDbType.Int)
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
        Public Shared Function FindOrganizations(ByVal FiltroIscrizione As FiltroRicercaComunitaByIscrizione, ByVal LinguaID As Integer, ByVal PersonaID As Integer, ByVal ResponsabileID As Integer, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDs As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_FindOrganizations"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroByIscrizione", CType(FiltroIscrizione, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ResponsabileID", ResponsabileID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDs = objAccesso.GetdataSet(oRequest)

            Catch ex As Exception

            End Try
            Return oDs
        End Function

        Public Shared Function FindCommunityForManagement(ByVal LinguaID As Integer, ByVal FacoltaID As Integer, Optional ByVal Percorso As String = "", Optional ByVal CMNT_IdPadre As Integer = 0, Optional ByVal PRSN_Id As Integer = -1, Optional ByVal ResponsabileID As Integer = -1, Optional ByVal valore As String = "", Optional ByVal TipoComunitaID As Integer = -1, Optional ByVal TipoCorsoDiStudiID As Integer = -1, Optional ByVal Anno As Integer = -1, Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Comunita_FindCommunityForManagement"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_IdPadre", CMNT_IdPadre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ResponsabileID", ResponsabileID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", FacoltaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Anno", Anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoCorsoDiStudiID", TipoCorsoDiStudiID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Percorso", Percorso, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
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

        Public Shared Function GetUsedRoles(ByVal idCommunity As Integer, ByVal idLanguage As Integer) As List(Of COL_TipoRuolo)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iReader As IDataReader = Nothing
            Dim objAccesso As New COL_DataAccess
            Dim items As New List(Of COL_TipoRuolo)

            With oRequest
                .Command = "sp_Comunita_ElencaRuoliAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", idCommunity, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(FiltroRuoli.ForUtenti_NoGuest, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(FiltroUtenti.NoPassantiNoCreatori, FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", idLanguage, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusoUtenteID", 0, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iReader = objAccesso.GetdataReader(oRequest)

                Try
                    While iReader.Read
                        items.Add(New COL_TipoRuolo(GenericValidator.ValInteger(iReader.Item("TPRL_ID"), 0), GenericValidator.ValString(iReader.Item("TPRL_nome"), "")))
                    End While
                Catch ex As Exception

                Finally
                    If Not iReader.IsClosed Then
                        iReader.Close()
                    End If
                End Try
            Catch ex As Exception
                If Not IsNothing(iReader) AndAlso Not iReader.IsClosed Then
                    iReader.Close()
                End If
            End Try
            Return items
        End Function
    End Class
End Namespace