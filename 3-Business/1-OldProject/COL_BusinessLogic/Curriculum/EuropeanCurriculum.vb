Imports COL_DataLayer

Namespace Curriculum_Europeo
    Public Class COL_CurriculumEuropeo

#Region "Private Property"
        Private n_CREU_id As Integer
        Private n_CREU_PRSN_id As Integer
        Private n_CREU_nome As String
        Private n_CREU_cognome As String
        Private n_CREU_dataNascita As DateTime
        Private n_CREU_sesso As Integer
        Private n_CREU_indirizzo As String
        Private n_CREU_cap As String
        Private n_CREU_citta As String
        Private n_CREU_telefono As String
        Private n_CREU_cellulare As String
        Private n_CREU_fax As String
        Private n_CREU_mail As String
        Private n_CREU_nazionalita As String
        Private n_CREU_madreLingua As String
        Private n_CREU_competenzeRelazionali As String
        Private n_CREU_competenzeOrganizzative As String
        Private n_CREU_competenzeTecniche As String
        Private n_CREU_competenzeArtistiche As String
        Private n_CREU_altreCompetenze As String
        Private n_CREU_patente As String
        Private n_CREU_ulterioriInfo As String
        Private n_CREU_CreatoIl As DateTime
        Private n_CREU_ModificatoIl As DateTime
        Private n_CREU_rendiPubblico As Integer
        Private n_CREU_mostraDatiSensibili As Integer
        Private n_CREU_mostraRecapiti As Integer
        Private n_ErroreDB As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_CREU_id
            End Get
            Set(ByVal Value As Integer)
                n_CREU_id = Value
            End Set
        End Property
        Public Property PRSN_ID() As Integer
            Get
                PRSN_ID = n_CREU_PRSN_id
            End Get
            Set(ByVal Value As Integer)
                n_CREU_PRSN_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_CREU_nome
            End Get
            Set(ByVal Value As String)
                n_CREU_nome = Value
            End Set
        End Property
        Public Property Cognome() As String
            Get
                Cognome = n_CREU_cognome
            End Get
            Set(ByVal Value As String)
                n_CREU_cognome = Value
            End Set
        End Property
        Public Property DataNascita() As DateTime
            Get
                DataNascita = n_CREU_dataNascita
            End Get
            Set(ByVal Value As DateTime)
                n_CREU_dataNascita = Value
            End Set
        End Property
        Public Property Sesso() As Integer
            Get
                Sesso = n_CREU_sesso
            End Get
            Set(ByVal Value As Integer)
                n_CREU_sesso = Value
            End Set
        End Property
        Public Property Indirizzo() As String
            Get
                Indirizzo = n_CREU_indirizzo
            End Get
            Set(ByVal Value As String)
                n_CREU_indirizzo = Value
            End Set
        End Property
        Public Property Cap() As String
            Get
                Cap = n_CREU_cap
            End Get
            Set(ByVal Value As String)
                n_CREU_cap = Value
            End Set
        End Property
        Public Property Citta() As String
            Get
                Citta = n_CREU_citta
            End Get
            Set(ByVal Value As String)
                n_CREU_citta = Value
            End Set
        End Property
        Public Property Telefono() As String
            Get
                Telefono = n_CREU_telefono
            End Get
            Set(ByVal Value As String)
                n_CREU_telefono = Value
            End Set
        End Property
        Public Property Cellulare() As String
            Get
                Cellulare = n_CREU_cellulare
            End Get
            Set(ByVal Value As String)
                n_CREU_cellulare = Value
            End Set
        End Property
        Public Property Fax() As String
            Get
                Fax = n_CREU_fax
            End Get
            Set(ByVal Value As String)
                n_CREU_fax = Value
            End Set
        End Property
        Public Property Mail() As String
            Get
                Mail = n_CREU_mail
            End Get
            Set(ByVal Value As String)
                n_CREU_mail = Value
            End Set
        End Property
        Public Property Nazionalita() As String
            Get
                Nazionalita = n_CREU_nazionalita
            End Get
            Set(ByVal Value As String)
                n_CREU_nazionalita = Value
            End Set
        End Property
        Public Property Madrelingua() As String
            Get
                Madrelingua = n_CREU_madreLingua
            End Get
            Set(ByVal Value As String)
                n_CREU_madreLingua = Value
            End Set
        End Property
        Public Property CompetenzeRelazionali() As String
            Get
                CompetenzeRelazionali = n_CREU_competenzeRelazionali
            End Get
            Set(ByVal Value As String)
                n_CREU_competenzeRelazionali = Value
            End Set
        End Property
        Public Property CompetenzeOrganizzative() As String
            Get
                CompetenzeOrganizzative = n_CREU_competenzeOrganizzative
            End Get
            Set(ByVal Value As String)
                n_CREU_competenzeOrganizzative = Value
            End Set
        End Property
        Public Property CompetenzeTecniche() As String
            Get
                CompetenzeTecniche = n_CREU_competenzeTecniche
            End Get
            Set(ByVal Value As String)
                n_CREU_competenzeTecniche = Value
            End Set
        End Property
        Public Property CompetenzeArtistiche() As String
            Get
                CompetenzeArtistiche = n_CREU_competenzeArtistiche
            End Get
            Set(ByVal Value As String)
                n_CREU_competenzeArtistiche = Value
            End Set
        End Property
        Public Property AltreCompetenze() As String
            Get
                AltreCompetenze = n_CREU_altreCompetenze
            End Get
            Set(ByVal Value As String)
                n_CREU_altreCompetenze = Value
            End Set
        End Property
        Public Property Patente() As String
            Get
                Patente = n_CREU_patente
            End Get
            Set(ByVal Value As String)
                n_CREU_patente = Value
            End Set
        End Property
        Public Property UlterioriInfo() As String
            Get
                UlterioriInfo = n_CREU_ulterioriInfo
            End Get
            Set(ByVal Value As String)
                n_CREU_ulterioriInfo = Value
            End Set
        End Property


        Public ReadOnly Property CreatoIl() As DateTime
            Get
                CreatoIl = n_CREU_CreatoIl
            End Get
        End Property
        Public ReadOnly Property ModificatoIl() As DateTime
            Get
                ModificatoIl = n_CREU_ModificatoIl
            End Get
        End Property

        Public Property RendiPubblico() As Boolean
            Get
                RendiPubblico = (Me.n_CREU_rendiPubblico = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_CREU_rendiPubblico = 1
                Else
                    n_CREU_rendiPubblico = 0
                End If
            End Set
        End Property
        Public Property MostraDatiSensibili() As Boolean
            Get
                MostraDatiSensibili = n_CREU_mostraDatiSensibili
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    Me.n_CREU_mostraDatiSensibili = 1
                Else
                    Me.n_CREU_mostraDatiSensibili = 0
                End If
            End Set
        End Property
        Public Property MostraRecapiti() As Boolean
            Get
                MostraRecapiti = n_CREU_mostraRecapiti
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    Me.n_CREU_mostraRecapiti = 1
                Else
                    Me.n_CREU_mostraRecapiti = 0
                End If
            End Set
        End Property

        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_ErroreDB
            End Get
        End Property
#End Region

        Sub New()
            Me.n_ErroreDB = Errori_Db.None
        End Sub

#Region "Metodi"
        Public Function Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Curriculum_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CREU_id", Me.n_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nome", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cognome", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_dataNascita", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_sesso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_indirizzo", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cap", "", ParameterDirection.Output, SqlDbType.VarChar, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_citta", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_telefono", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cellulare", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_fax", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mail", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nazionalita", "", ParameterDirection.Output, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_madreLingua", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeRelazionali", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeOrganizzative", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeTecniche", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeArtistiche", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_altreCompetenze", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_patente", "", ParameterDirection.Output, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ulterioriInfo", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_CreatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ModificatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraDatiSensibili", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraRecapiti", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
           
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CREU_PRSN_id = oRequest.GetValueFromParameter(2)
                Me.n_CREU_nome = oRequest.GetValueFromParameter(3)
                Me.n_CREU_cognome = oRequest.GetValueFromParameter(4)
                If IsDate(oRequest.GetValueFromParameter(5)) Then
                    Me.n_CREU_dataNascita = CDate(oRequest.GetValueFromParameter(5))
                End If
                Me.n_CREU_sesso = oRequest.GetValueFromParameter(6)
                Me.n_CREU_indirizzo = oRequest.GetValueFromParameter(7)
                Me.n_CREU_cap = oRequest.GetValueFromParameter(8)
                Me.n_CREU_citta = oRequest.GetValueFromParameter(9)
                Me.n_CREU_telefono = oRequest.GetValueFromParameter(10)
                Me.n_CREU_cellulare = oRequest.GetValueFromParameter(11)
                Me.n_CREU_fax = oRequest.GetValueFromParameter(12)

                Me.n_CREU_mail = oRequest.GetValueFromParameter(13)
                Me.n_CREU_nazionalita = oRequest.GetValueFromParameter(14)
                Me.n_CREU_madreLingua = oRequest.GetValueFromParameter(15)
                Me.n_CREU_competenzeRelazionali = oRequest.GetValueFromParameter(16)
                Me.n_CREU_competenzeOrganizzative = oRequest.GetValueFromParameter(17)
                Me.n_CREU_competenzeTecniche = oRequest.GetValueFromParameter(18)
                Me.n_CREU_competenzeArtistiche = oRequest.GetValueFromParameter(19)
                Me.n_CREU_altreCompetenze = oRequest.GetValueFromParameter(20)
                Me.n_CREU_patente = oRequest.GetValueFromParameter(21)
                Me.n_CREU_ulterioriInfo = oRequest.GetValueFromParameter(22)
                If IsDate(oRequest.GetValueFromParameter(23)) Then
                    Me.n_CREU_CreatoIl = CDate(oRequest.GetValueFromParameter(23))
                End If
                If IsDate(oRequest.GetValueFromParameter(24)) Then
                    Me.n_CREU_ModificatoIl = CDate(oRequest.GetValueFromParameter(24))
                End If
                Me.n_CREU_rendiPubblico = oRequest.GetValueFromParameter(25)
                Me.n_CREU_mostraDatiSensibili = oRequest.GetValueFromParameter(26)
                Me.n_CREU_mostraRecapiti = oRequest.GetValueFromParameter(27)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
                responso = -1
            End Try
            Return responso

        End Function
        Public Function EstraiByPersona(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Curriculum_EstraiByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_PRSN_id", PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nome", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cognome", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_dataNascita", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_sesso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_indirizzo", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cap", "", ParameterDirection.Output, SqlDbType.VarChar, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_citta", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_telefono", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cellulare", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_fax", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mail", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nazionalita", "", ParameterDirection.Output, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_madreLingua", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeRelazionali", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeOrganizzative", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeTecniche", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeArtistiche", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_altreCompetenze", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_patente", "", ParameterDirection.Output, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ulterioriInfo", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_CreatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ModificatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraDatiSensibili", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraRecapiti", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(1) <> "" Then
                    Me.n_CREU_id = oRequest.GetValueFromParameter(1)
                    Me.n_CREU_nome = oRequest.GetValueFromParameter(3)
                    Me.n_CREU_cognome = oRequest.GetValueFromParameter(4)
                    If IsDate(oRequest.GetValueFromParameter(5)) Then
                        Me.n_CREU_dataNascita = CDate(oRequest.GetValueFromParameter(5))
                    End If
                    Me.n_CREU_sesso = oRequest.GetValueFromParameter(6)
                    Me.n_CREU_indirizzo = oRequest.GetValueFromParameter(7)
                    Me.n_CREU_cap = oRequest.GetValueFromParameter(8)
                    Me.n_CREU_citta = oRequest.GetValueFromParameter(9)
                    Me.n_CREU_telefono = oRequest.GetValueFromParameter(10)
                    Me.n_CREU_cellulare = oRequest.GetValueFromParameter(11)
                    Me.n_CREU_fax = oRequest.GetValueFromParameter(12)

                    Me.n_CREU_mail = oRequest.GetValueFromParameter(13)
                    Me.n_CREU_nazionalita = oRequest.GetValueFromParameter(14)
                    Me.n_CREU_madreLingua = oRequest.GetValueFromParameter(15)
                    Me.n_CREU_competenzeRelazionali = oRequest.GetValueFromParameter(16)
                    Me.n_CREU_competenzeOrganizzative = oRequest.GetValueFromParameter(17)
                    Me.n_CREU_competenzeTecniche = oRequest.GetValueFromParameter(18)
                    Me.n_CREU_competenzeArtistiche = oRequest.GetValueFromParameter(19)
                    Me.n_CREU_altreCompetenze = oRequest.GetValueFromParameter(20)
                    Me.n_CREU_patente = oRequest.GetValueFromParameter(21)
                    Me.n_CREU_ulterioriInfo = oRequest.GetValueFromParameter(22)
                    If IsDate(oRequest.GetValueFromParameter(23)) Then
                        Me.n_CREU_CreatoIl = CDate(oRequest.GetValueFromParameter(23))
                    End If
                    If IsDate(oRequest.GetValueFromParameter(24)) Then
                        Me.n_CREU_ModificatoIl = CDate(oRequest.GetValueFromParameter(24))
                    End If
                    Me.n_CREU_rendiPubblico = oRequest.GetValueFromParameter(25)
                    Me.n_CREU_mostraDatiSensibili = oRequest.GetValueFromParameter(26)
                    Me.n_CREU_mostraRecapiti = oRequest.GetValueFromParameter(27)
                    Me.n_ErroreDB = Errori_Db.None

                Else
                    Me.n_CREU_id = -1
                    Me.n_ErroreDB = Errori_Db.DBReadExist
                End If


            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
                responso = -1
            End Try
            Return responso

        End Function
        Public Function Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Curriculum_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_PRSN_id", Me.n_CREU_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nome", Me.n_CREU_nome, ParameterDirection.Input, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cognome", Me.n_CREU_cognome, ParameterDirection.Input, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)

                If Equals(New Date, Me.n_CREU_dataNascita) = False Then
                    oParam = objAccesso.GetAdvancedParameter("@CREU_dataNascita", DateToString(Me.n_CREU_dataNascita, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                    .Parameters.Add(oParam)
                End If
                oParam = objAccesso.GetAdvancedParameter("@CREU_sesso", Me.n_CREU_sesso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_indirizzo", Me.n_CREU_indirizzo, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cap", Me.n_CREU_cap, ParameterDirection.Input, SqlDbType.VarChar, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_citta", Me.n_CREU_citta, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_telefono", Me.n_CREU_telefono, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cellulare", Me.n_CREU_cellulare, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_fax", Me.n_CREU_fax, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mail", Me.n_CREU_mail, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nazionalita", Me.n_CREU_nazionalita, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_madreLingua", Me.n_CREU_madreLingua, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeRelazionali", Me.n_CREU_competenzeRelazionali, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeOrganizzative", Me.n_CREU_competenzeOrganizzative, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeTecniche", Me.n_CREU_competenzeTecniche, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeArtistiche", Me.n_CREU_competenzeArtistiche, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_altreCompetenze", Me.n_CREU_altreCompetenze, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_patente", Me.n_CREU_patente, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ulterioriInfo", Me.n_CREU_ulterioriInfo, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                If Equals(New Date, Me.n_CREU_CreatoIl) = False Then
                    oParam = objAccesso.GetAdvancedParameter("@CREU_CreatoIl", DateToString(Me.n_CREU_CreatoIl, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                    .Parameters.Add(oParam)
                End If
                'If Equals(New Date, Me.n_CREU_ModificatoIl) = False Then
                '    oParam = objAccesso.GetAdvancedParameter("@CREU_ModificatoIl", DateToString(Me.n_CREU_ModificatoIl, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                '    .Parameters.Add(oParam)
                'End If

                oParam = objAccesso.GetAdvancedParameter("@CREU_rendiPubblico", Me.n_CREU_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraDatiSensibili", Me.n_CREU_mostraDatiSensibili, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraRecapiti", Me.n_CREU_mostraRecapiti, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_CREU_id = oRequest.GetValueFromParameter(1)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_CREU_id = -1
                Me.n_ErroreDB = Errori_Db.DBInsert
                responso = -1
            End Try
            Return responso

        End Function
        Public Function Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Curriculum_Modifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CREU_id", Me.n_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_PRSN_id", Me.n_CREU_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nome", Me.n_CREU_nome, ParameterDirection.Input, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cognome", Me.n_CREU_cognome, ParameterDirection.Input, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)

                If Equals(New Date, Me.n_CREU_dataNascita) = False Then
                    oParam = objAccesso.GetAdvancedParameter("@CREU_dataNascita", DateToString(Me.n_CREU_dataNascita, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                    .Parameters.Add(oParam)
                End If
                oParam = objAccesso.GetAdvancedParameter("@CREU_sesso", Me.n_CREU_sesso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_indirizzo", Me.n_CREU_indirizzo, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cap", Me.n_CREU_cap, ParameterDirection.Input, SqlDbType.VarChar, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_citta", Me.n_CREU_citta, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_telefono", Me.n_CREU_telefono, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_cellulare", Me.n_CREU_cellulare, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_fax", Me.n_CREU_fax, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mail", Me.n_CREU_mail, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_nazionalita", Me.n_CREU_nazionalita, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_madreLingua", Me.n_CREU_madreLingua, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeRelazionali", Me.n_CREU_competenzeRelazionali, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeOrganizzative", Me.n_CREU_competenzeOrganizzative, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeTecniche", Me.n_CREU_competenzeTecniche, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_competenzeArtistiche", Me.n_CREU_competenzeArtistiche, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_altreCompetenze", Me.n_CREU_altreCompetenze, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_patente", Me.n_CREU_patente, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_ulterioriInfo", Me.n_CREU_ulterioriInfo, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
                .Parameters.Add(oParam)
                'If Equals(New Date, Me.n_CREU_CreatoIl) = False Then
                '    oParam = objAccesso.GetAdvancedParameter("@CREU_CreatoIl", DateToString(Me.n_CREU_CreatoIl, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                '    .Parameters.Add(oParam)
                'End If
                If Equals(New Date, Me.n_CREU_ModificatoIl) = False Then
                    oParam = objAccesso.GetAdvancedParameter("@CREU_ModificatoIl", DateToString(Me.n_CREU_ModificatoIl, True), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                    .Parameters.Add(oParam)
                End If

                oParam = objAccesso.GetAdvancedParameter("@CREU_rendiPubblico", Me.n_CREU_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraDatiSensibili", Me.n_CREU_mostraDatiSensibili, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CREU_mostraRecapiti", Me.n_CREU_mostraRecapiti, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBChange
                responso = -1
            End Try
            Return responso

        End Function
        Public Function Elimina(ByVal CREU_id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Curriculum_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CREU_id", CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
#End Region

        Public Sub GetNumeroElementiAssociati(ByVal CREU_id As Integer, ByRef numIstruzione As Integer, ByRef numLingua As Integer, ByRef numEsperienze As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            'Dim NumEsperienze As Integer

            With oRequest
                .Command = "sp_Curriculum_ElementiAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CREU_id", CREU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@NumIstruzione", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@NumLingua", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@NumEsperienze", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                numIstruzione = oRequest.GetValueFromParameter(2)
                numLingua = oRequest.GetValueFromParameter(3)
                numEsperienze = oRequest.GetValueFromParameter(4)
                Me.n_ErroreDB = Errori_Db.None
            Catch ax As Exception 'record non trovato
                numEsperienze = 0
                numIstruzione = 0
                numLingua = 0
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
            ' Return NumEsperienze
        End Sub




    End Class
End Namespace

