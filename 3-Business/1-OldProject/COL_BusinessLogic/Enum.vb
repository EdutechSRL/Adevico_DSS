
Public Module Main
    <Serializable()>
     Public Enum ConfigFileType
        File = 0
        FileAree = 1
        VideoCast = 2
        Scorm = 3
        Cover = 4
        Mail = 5
        MailAree = 6
        Profilo = 7
        Wiki = 8
        FileTesi = 9
        Bacheca = 10
        Questionari = 11
    End Enum
    'Public Enum TipoMaterialeUpload
    '	Standard = 1
    '	Scorm = 2
    '	VideoCast = 3
    '	ScormOrVideoCast = 4
    'End Enum
    <Serializable()>
     Public Enum Errori_Db As Integer
        None = 0            ' Nessun errore
        DBInsert = 1        ' Mancato inserimento
        DBError = 2         ' Errore accesso al DB
        DBChange = 3        ' Non esistente
        DBDelete = 4        ' mancato cancellamento
        DBReadExist = 5     ' mancato record ricercato
        System = 6          ' errore di sistema
    End Enum
    <Serializable()>
     Public Enum Errore_File As Integer
        none = 0            ' nessun errore
        exsist = 1          ' file gi‡ inserito
        notUploaded = 2     ' mancato upload
        zeroByte = 3        ' lunghezza file nulla
        dirNotFound = 4     ' directory non trovata
        fileNotFound = 5    ' file non trovato
        fileMismatchType = 6 ' tipo 
    End Enum
    <Serializable()>
     Public Enum Abilitazione As Integer
        Tutti = 0 'abilitati si e no, e attivati si e no
        AttivatoAbilitato = 1 'tutti gli abilitati and tutti gli attivati
        Attivato = 2 'solo gli attivati
        Abilitato = 3 ' solo gli abilitati
        NonAttivato = 4 ' i non attivati
        NonAbilitato = 5 ' i non abilitati
        NonAttivatoNonAbilitato = 6 'tutti i non attivati and i non attivati
        NonAbilitatoAttivato = 7 ' solo quelli disabilitati (quelli che fanno casino)
    End Enum
    <Serializable()>
     Public Enum ARRControlli As Integer
        'era la vecchia storia per caricare i controlli a runtime!!!
        'vedere se Ë ancora utile
        'posizioni dell'Array:
        Header = 0
        Footer = 1
        Destro = 2
        Sinistro = 3
        Centrale = 4
    End Enum
    <Serializable()>
     Public Enum AuthenticationType As Integer
        'indica i vari metodi possibili per autenticarsi/registrarsi
        Base = 0 'Ë il metodo di autenticazione del sistema
        Comol = 1
        LDAP = 2
    End Enum
    <Serializable()>
     Public Enum TipoRicercaArea As Integer
        Nulla = 0
        NomeArea = 1
        CognomeNome = 2
        DataCreazioneDopo = 3
        DataCreazionePrima = 4
    End Enum

    <Serializable>
Public Enum ErroreRegistrazione As Integer
        none = 0
        InserimentoAvvenuto = 1
        InserimentoElogon = 2
        LoginGi‡Assegnata = -1
        CodiceFiscaleGi‡Assegnato = -2
        MailGi‡Assegnata = -3
        MailNonInviata = -4
        ComunitaOrganizzazioneNonAssociata = -5
        OrganizzazioneNonAssociata = -6
        ErroreGenerico = -7
        Gi‡Inserito = -8
        FillField = -9
        MailSecondariaGi‡Assegnata = -11
        MatricolaGi‡Esistente = -10
        CambioPersonaAvvenuto = 3
        ModificaAvvenuta = 4
        InseritoBloccato = 5
        InseritoBloccatoInAttesa = 6
    End Enum
    <Serializable>
    Public Enum ErroriAutenticazione As Integer
        MailInviata = 0
        MailNonInviata = 1
        PasswordErrata = 2
        AccountDisabilitato = 3
        LogonBloccato = 4
        AccountNonAttivato = 5
        AccountNonRegistrato = 6
        AccessoLiberoConsentito = 7
        NoConnessioneDB = -1
        NoConnessioneLDAP = -2
        NoConnessioneAlcuniLDAP = -3
        AccessoLiberoNonConsentito = -4
        AccessoLiberoNoComunita = -5
    End Enum
    <Serializable>
    Public Enum VelocitaScorrimento As Integer
        Fermo = 0
        Lento = 1
        Veloce = 2
        MoltoVeloce = 3
    End Enum
    <Serializable>
    Public Enum ErroriIscrizioneComunita As Integer
        Nessuno = 0
        ErroreGenerico = -1
        IscrizioneAvvenuta = 1
        IscrizioneInAttesa = 2
        LimiteSuperato = -2
    End Enum

    <Serializable>
    Public Enum TipoAutenticazione As Integer
        Tutti = -1
        ComunitaOnLine = 1
        LDAP = 2
        IOP = 3
    End Enum
    <Serializable>
    Public Enum TipoScadenzaPassword As Integer
        Valida = 1
        Scaduta = 2
        ScadutaDal = 3
        ScadeIl = 4

    End Enum
    <Serializable>
    Public Enum TipoAttivazione As Integer
        Tutti = -1
        Attivati = 1
        InAttesa = 0
        Bloccati = 2
        AccountNonConfermato = 3
        NuoviIscritti = 4
    End Enum
    <Serializable>
    Public Enum TipoComunitaStandard As Integer
        Tutti = -1
        Portale = -2
        Organizzazione = 0
        C1 = 1
        C2 = 2
        C3 = 3
        C4 = 4
        GruppoDiLavoro = 5
        C6 = 6
        C7 = 7
    End Enum
    <Serializable>
    Public Enum TipoPersonaStandard As Integer
        Tutti_WithGuest = -2
        Tutti_NoGuest = -1
        Studente = 1
        Docente = 2
        Tutor = 3
        Esterno = 4
        Altro = 5
        Amministrativo = 6
        SysAdmin = 7
        Copisteria = 8
        Dottorando = 9
        StudenteSuperiori = 10
        DocenteSuperiori = 11
        Direttore = 12
        Ricercatore = 13
        DocenteStandard = 14
        StudenteStandard = 15
        ExStudente = 16
        Tecnico = 17
        Guest = 18
        AdminSecondario = 19
        Company = 20
        PublicUser = 21
    End Enum
    <Serializable>
    Public Enum TipoRuoloStandard As Integer
        Tutti_WithGuest = -5
        Tutti_NoGuest = -1
        AccessoNonAutenticato = -4
        Passante = -3
        Creatore = -2
        AdminComunit‡ = 1
        AdminGlobale = 2
        AdminOrganizzazione = 3
        Guest = 4
        Docente = 5
        Studente = 6
        Tesista = 7
        Tutor = 8
        Segretario = 10
        Esterno = 16
        Copisteria = 17
        Collaboratore = 18
        CapoProgetto = 19
        Ricercatore = 20
        Dottorando = 21
        Direttore = 22
        StudenteSuperiori = 27
        DocenteSuperiori = 28
        StudenteStandard = 29
        DocenteStandard = 30
        ExStudente = 31
        Tecnico = 32
    End Enum
    <Serializable>
    Public Enum TipoTesiStandard As Integer
        Tesi_Triennale = 1
        Tesi_Specialistica = 2
        Curriculare_Laurea_Quadriennale = 3
        Compilativa_Laurea_Quadriennale = 4
        Documentaristica_Laurea_Quadriennale = 5
        Ricerca_Laurea_Quadriennale = 6
        Breve_Laurea_Quadriennale = 7
    End Enum
    <Serializable>
    Public Enum LivelloTesi As Integer
        Tutti = -1
        Triennale = 1
        Specialistica = 2
        VecchioOrdinamento = 3
        Dottorato = 4
    End Enum
    <Serializable>
    Public Enum TipiPostIt As Integer
        tutti = -1
        Altro = 0
        NotaPersonale = 1
        NotaComunit‡ = 2
        NotaApersona = 3
    End Enum
    <Serializable>
    Public Enum TipiEvento As Integer
        tutti = -1
        Reminder = 0
        Lezione = 1
        AppelloEsame = 2
        SessioneLaurea = 3
        Vacanze = 4
        ConsiglioRiunione = 5
        Altro = 6
    End Enum
    <Serializable>
    Public Enum TipoDestinatarioMail As Integer
        A = 1
        CC = 2
        CCN = 3
    End Enum
    <Serializable>
    Public Enum TipiCorsoDiStudiStandard As Integer
        VecchioOrdinamento = 1
        Triennale = 2
        Specialistica = 3
        Dottorato = 4
        PostLauream = 5
    End Enum
    <Serializable>
    Public Enum RuoloForumStandard As Integer
        NotSet = 0
        Amministratore = 1
        Moderatore = 2
        Partecipante = 3
        Ospite = 4
    End Enum
    <Serializable>
    Public Enum PostApprovazione As Integer
        Approvato = 1
        InAttesa = 2
        Censurato = 3
    End Enum
    <Serializable>
    Public Enum FiltroPostApprovazione As Integer
        Tutti = 0
        Approvato = 1
        InAttesa = 2
        Censurato = 3
        ApprovatoCensurato = 4
        ApprovatoInAttesa = 5
        Approvati_PropriCens_Attesa = 6
    End Enum
    <Serializable>
    Public Enum FiltroStatoComunita As Integer
        Tutte = -1
        Attiva = 0
        Archiviata = 1
        Bloccata = 2
    End Enum
    <Serializable>
    Public Enum FiltroStatoTesi As Integer
        Tutte = -1
        NonAssegnata = 1
        InCorso = 2
        Sospesa = 3
        Conclusa = 4
        Abbandonata = 5
        Accettata = 6
        Rifiutata = 7
        NonEsaminata = 8
        NonAssegnataNonEsaminata = 9
    End Enum
    <Serializable>
    Public Enum FiltroMailDestinatari As Integer
        Tutti = -1
        nonOriginali = 0
        Originali = 1
    End Enum
    Public Enum FiltroVisibilit‡ As Integer
        Tutti = -1
        NonVisibile = 0
        Visibile = 1
        Pubblici = 2
        Deleted = 3
    End Enum
    Public Enum FiltroArchiviazione As Integer
        Tutti = -1
        Archiviato = 1
        NonArchiviato = 0
    End Enum
    Public Enum FiltroPubblicazione As Integer
        Tutti = -1
        NonPubblicabili = 0
        Pubblicabili = 1
    End Enum
    Public Enum FiltroComunita As Integer
        IDresponsabile = -9
        cognomeDocente = -8
        dataFineIscrizionePrima = -6
        dataIscrizioneDopo = -5
        creataPrima = -4
        creataDopo = -3
        contiene = -7
        nome = -2
        tutti = -1
        altro = 0
        a = 1
        b = 2
        c = 3
        d = 4
        e = 5
        f = 6
        g = 7
        h = 8
        i = 9
        j = 10
        k = 11
        l = 12
        m = 13
        n = 14
        o = 15
        p = 16
        q = 17
        r = 18
        s = 19
        t = 20
        u = 21
        v = 22
        w = 23
        x = 24
        y = 25
        z = 26
    End Enum
    Public Enum FiltroAbilitazione As Integer
        Tutti = 0 'abilitati si e no, e attivati si e no
        AttivatoAbilitato = 1 'tutti gli abilitati and tutti gli attivati
        Attivato = 2 'solo gli attivati
        Abilitato = 3 ' solo gli abilitati
        NonAttivato = 4 ' i non attivati
        NonAbilitato = 5 ' i non abilitati
        NonAttivatoNonAbilitato = 6 'tutti i non attivati and i non attivati
        NonAbilitatoAttivato = 7 ' solo quelli disabilitati (quelli che fanno casino)
        TuttiUltimiIscritti = 8
        IscrizioneNonConfermata = 9
    End Enum
    Public Enum FiltroUtenti As Integer
        Tutti = 0
        NoPassanti = 1
        NoCreatori = 2
        NoPassantiNoCreatori = 3
        Passanti = 4
        Creatori = 5
        PassantiCreatori = 6
        Responsabili = 7
    End Enum
    Public Enum FiltroRuoli As Integer
        ForAll_NoGuest = 0
        ForTipoComunita_NoGuest = 1 'tutti i ruoli disponibili per quel tipo di comunit‡
        ForUtenti_NoGuest = 2 'solo i ruoli degli utenti iscritti
        ForTipoComunita_Padri_NoGuest = 3
        ForUtenti_ComunitaPadri_NoGuest = 4
        ForProfiloComunita_NoGuest = 5

        ForAll = -1
        ForTipoComunita = 7 'tutti i ruoli disponibili per quel tipo di comunit‡
        ForUtenti = 8 'solo i ruoli degli utenti iscritti
        ForTipoComunita_Padri = 9
        ForUtenti_ComunitaPadri = 10
        ForProfiloComunita = 11
    End Enum
    Public Enum FiltroElencoTipiPersona As Integer
        All = 0
        All_NoGuest = 1
        WithUserAssociated = 2
        WithUserAssociated_NoGuest = 3
    End Enum
    Public Enum FiltroRicercaAnagrafica As Integer
        login = -7
        matricola = -6
        dataNascita = -5
        nomeCognome = -4
        cognome = -3
        nome = -2
        tutti = -1
    End Enum

    Public Enum ProfileStatusFilter As Integer
        all = -1
        onlyactive = 1
        allnotavailable = 0
        onlywaiting = 2
        onlydisabled = 3
    End Enum
    Public Enum FiltroAnagrafica As Integer
        login = -7
        matricola = -6
        dataNascita = -5
        nomeCognome = -4
        cognome = -3
        nome = -2
        tutti = -1
        altro = 0
        a = 1
        b = 2
        c = 3
        d = 4
        e = 5
        f = 6
        g = 7
        h = 8
        i = 9
        j = 10
        k = 11
        l = 12
        m = 13
        n = 14
        o = 15
        p = 16
        q = 17
        r = 18
        s = 19
        t = 20
        u = 21
        v = 22
        w = 23
        x = 24
        y = 25
        z = 26
    End Enum
    Public Enum FiltroSelezioneForum As Integer
        Forum = 1
        ForumTopic = 2
        ForumTopicPost = 3
    End Enum
    Public Enum FiltroPersona As Integer
        tutte = -1
        nome = 1
        cognome = 2
        dataNascita = 3
        matricola = 4
        mail = 5
        codiceFiscale = 6
        login = 7
    End Enum

    Public Enum FiltroRicercaComunitaByIscrizione As Integer
        tutte = -1
        nonIscritto = 0
        iscritto = 1
        forAdmin = 2
    End Enum
    Public Enum FiltroPostit As Integer
        Tutti = -1
        NotePersonali = 0
        Ricevuti = 1
        Inviati = 2
    End Enum
    Public Enum FiltroCampoOrdinePersona As Integer
        anagrafica = 1
        dataNascita = 2
        tipoPersona = 3
        mail = 4
        login = 5
        indirizzoIP = 6
        browser = 7
        dataConnessione = 8
        nome = 9
        cognome = 10
        dataIscrizione = 11
        RuoloComunita = 12
        RuoloForum = 13
    End Enum
    Public Enum FiltroCampoOrdineComunita As Integer
        Nessuno = 0
        Nome = 1
        DataCreazione = 2
        DataInizioIscrizione = 3
        DataFineIscrizione = 4
        IsChiusa = 5
        TipoComunita = 6
        Creatore = 7
        Responsabile = 8
        Livello = 9
        AnnoAccademico = 9
        Periodo = 9
    End Enum
    Public Enum FiltroCampoOrdineTesi As Integer
        titolo = 1
        tipoTesi = 2
        relatore = 3
        tesista = 4
    End Enum
    Public Enum FiltroMenuLimbo As Integer
        LimboStandard = 0
        LimboForAdmin = 1
    End Enum
    Public Enum FiltroAree As Integer
        Tutte = -1
        Create = 0
        Sottoscritte = 1
        NonSottoScritte = 2
    End Enum
    Public Enum FiltroFile As Integer
        soloFile = 1
        soloCartelle = 0
        tutti = -1
    End Enum
    Public Enum FiltroProprietarioFile As Integer
        conProprietario = 1
        senzaProprietario = 0
        tutti = -1
    End Enum
    Public Enum FiltroOrdinamento As Integer
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Public Enum FiltroOrdineCommissioni As Integer
        nessuno = 0
        dataAppello = 1
        dataCreazione = 2
        corsoDiStudi = 3
        tipoCommissione = 4
        annoAccademico = 5
    End Enum
    Public Enum ElencoRecord As Integer
        Normale = 0
        AdAlbero = 1
        AdAlberoOrganizzativo = 2
    End Enum
    Public Enum FiltroRicercaEventi As Integer
        CompresiTraDueDate = 0
        PrimaDiUnaData = 1
        DopoUnaData = 2
        InUnaDataSpecifica = 3
        TutteLeDate = 4
        EsclusiCompresiTraDueDate = 5
        ModificatiDopoIl = 6
    End Enum
    Public Enum DeiscrizioneUtenti As Integer
        Nessuno = 0
        NonCoordinati_Completa = 1
        NonCoordinati_EsclusiDocumenti = 2
    End Enum
    Public Enum CategoriaFile As Integer
        Cartella = -2
        NonSpecificata = 0
        PacchettoScorm = 100000
        VideoCast = 100000
    End Enum

    'Public Enum TipoDestinatarioMail As Integer
    '    address_TO = 0
    '    address_CC = 1
    '    address_CCN = 2
    'End Enum

    Function DateToString(ByVal oData As DateTime, Optional ByVal oTime As Boolean = True) As String
        Dim response As String
        Dim ProviderDateTime As String
        ProviderDateTime = System.Configuration.ConfigurationManager.AppSettings("ProviderDateTime")

        If Equals(oData, Nothing) Then
            response = ""
        Else
            If IsNothing(ProviderDateTime) Then
                ProviderDateTime = "it"
            ElseIf ProviderDateTime = "" Then
                ProviderDateTime = "it"
            End If
            If LCase(ProviderDateTime) = "it" Then
                If oTime Then
                    response = Format(oData, "dd/MM/yyyy HH:mm:ss")
                Else
                    response = Format(oData, "dd/MM/yyyy")
                End If
            ElseIf LCase(ProviderDateTime) = "en" Then
                If oTime Then
                    response = Format(oData, "MM/dd/yyyy HH:mm:ss")
                Else
                    response = Format(oData, "MM/dd/yyyy")
                End If
            Else
                If oTime Then
                    response = Format(oData, "dd/MM/yyyy HH:mm:ss")
                Else
                    response = Format(oData, "dd/MM/yyyy")
                End If
            End If
            response = response.Replace(".", ":")
        End If
        Return response
    End Function
End Module