Namespace Comunita
    Public Class Iscritto
        'Private _PRSN_id As Integer
        'Private _PRSN_Nome As String
        'Private _PRSN_Cognome As String
        'Private _PRSN_Anagrafica As String
        'Private _PRSN_DataNascita As datetime
        'Private _PRSN_Cellulare As String
        'Private _PRSN_Mail As String
        'Private _PRSN_Login As String
        'Private _PRSN_fotoPath As String
        'Private _PRSN_mostraMail As Boolean

        Private _Persona As COL_Persona

        Private _PRSN_TPPR_id As Integer
        Private _TPPR_descrizione As String



        Private _RLPC_id
        Private _RLPC_ultimoCollegamento As datetime
        Private _RLPC_attivato As Boolean
        Private _RLPC_abilitato As Boolean
        Private _RLPC_Responsabile As Boolean
        Private _RLPC_IscrittoIl As datetime
        Private _RLPC_PRSN_mostraMail As Boolean


		Private _Ruolo As Role
        'Private _TPRL_id As Integer
        'Private _TPRL_Nome As String
        'Private _TPRL_gerarchia As Integer

        Private _STDN_matricola As String
        'Private _LKPO_Default As Integer
        Private Totale As Integer

        'Public Property PRSN_id() As Integer
        '    Get
        '        Return _PRSN_id
        '    End Get
        '    Set(ByVal value As Integer)
        '        _PRSN_id = value
        '    End Set
        'End Property
        'Public Property PRSN_Nome() As String
        '    Get
        '        Return _PRSN_Nome
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Nome = value
        '    End Set
        'End Property
        'Public Property PRSN_Cognome() As String
        '    Get
        '        Return _PRSN_Cognome
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Cognome = value
        '    End Set
        'End Property
        'Public Property PRSN_Anagrafica() As String
        '    Get
        '        Return _PRSN_Anagrafica
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Anagrafica = value
        '    End Set
        'End Property
        'Public Property PRSN_DataNascita() As datetime
        '    Get
        '        Return _PRSN_DataNascita
        '    End Get
        '    Set(ByVal value As datetime)
        '        _PRSN_DataNascita = value
        '    End Set
        'End Property
        'Public Property PRSN_Cellulare() As String
        '    Get
        '        Return _PRSN_Cellulare
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Cellulare = value
        '    End Set
        'End Property
        'Public Property PRSN_Mail() As String
        '    Get
        '        Return _PRSN_Mail
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Mail = value
        '    End Set
        'End Property
        'Public Property PRSN_Login() As String
        '    Get
        '        Return _PRSN_Login
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_Login = value
        '    End Set
        'End Property
        'Public Property PRSN_fotoPath() As String
        '    Get
        '        Return _PRSN_fotoPath
        '    End Get
        '    Set(ByVal value As String)
        '        _PRSN_fotoPath = value
        '    End Set
        'End Property
        'Public Property PRSN_mostraMail() As Boolean
        '    Get
        '        Return _PRSN_mostraMail
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _PRSN_mostraMail = value
        '    End Set
        'End Property

        Public Property Persona() As COL_Persona
            Get
                Return _Persona
            End Get
            Set(ByVal value As COL_Persona)
                _Persona = value
            End Set
        End Property
        Public Property PRSN_TPPR_id() As Integer
            Get
                Return _PRSN_TPPR_id
            End Get
            Set(ByVal value As Integer)
                _PRSN_TPPR_id = value
            End Set
        End Property
       
        Public Property RLPC_id() As Object
            Get
                Return _RLPC_id
            End Get
            Set(ByVal value As Object)
                _RLPC_id = value
            End Set
        End Property
        Public Property RLPC_ultimoCollegamento() As datetime
            Get
                Return _RLPC_ultimoCollegamento
            End Get
            Set(ByVal value As datetime)
                _RLPC_ultimoCollegamento = value
            End Set
        End Property
        Public Property RLPC_attivato() As Boolean
            Get
                Return _RLPC_attivato
            End Get
            Set(ByVal value As Boolean)
                _RLPC_attivato = value
            End Set
        End Property
        Public Property RLPC_abilitato() As Boolean
            Get
                Return _RLPC_abilitato
            End Get
            Set(ByVal value As Boolean)
                _RLPC_abilitato = value
            End Set
        End Property
        Public Property RLPC_Responsabile() As Boolean
            Get
                Return _RLPC_Responsabile
            End Get
            Set(ByVal value As Boolean)
                _RLPC_Responsabile = value
            End Set
        End Property

        Public Property RLPC_IscrittoIl() As datetime
            Get
                Return _RLPC_IscrittoIl
            End Get
            Set(ByVal value As datetime)
                _RLPC_IscrittoIl = value
            End Set
        End Property
        Public Property RLPC_PRSN_mostraMail() As Boolean
            Get
                Return _RLPC_PRSN_mostraMail
            End Get
            Set(ByVal value As Boolean)
                _RLPC_PRSN_mostraMail = value
            End Set
        End Property
		Public Property Ruolo() As Role
			Get
				Return _Ruolo
			End Get
			Set(ByVal value As Role)
				_Ruolo = value
			End Set
		End Property
        'Public Property TPRL_id() As Integer
        '    Get
        '        Return _TPRL_id
        '    End Get
        '    Set(ByVal value As Integer)
        '        _TPRL_id = value
        '    End Set
        'End Property
        'Public Property TPRL_Nome() As String
        '    Get
        '        Return _TPRL_Nome
        '    End Get
        '    Set(ByVal value As String)
        '        _TPRL_Nome = value
        '    End Set
        'End Property
        'Public Property TPPR_descrizione() As String
        '    Get
        '        Return _TPPR_descrizione
        '    End Get
        '    Set(ByVal value As String)
        '        _TPPR_descrizione = value
        '    End Set
        'End Property
        'Public Property TPRL_gerarchia() As Integer
        '    Get
        '        Return _TPRL_gerarchia
        '    End Get
        '    Set(ByVal value As Integer)
        '        _TPRL_gerarchia = value
        '    End Set
        'End Property
        Public Property STDN_matricola() As String
            Get
                Return _STDN_matricola
            End Get
            Set(ByVal value As String)
                _STDN_matricola = value
            End Set
        End Property
        'Public Property LKPO_Default() As Integer
        '    Get
        '        Return _LKPO_Default
        '    End Get
        '    Set(ByVal value As Integer)
        '        _LKPO_Default = value
        '    End Set
        'End Property
        Public Property Totale1() As Integer
            Get
                Return Totale
            End Get
            Set(ByVal value As Integer)
                Totale = value
            End Set
        End Property

        Public Sub New()

        End Sub
        'Public Sub New( _
        '    ByVal PRSN_id As Integer, _
        '    ByVal PRSN_Nome As String, _
        '    ByVal PRSN_Cognome As String, _
        '    ByVal PRSN_Anagrafica As String, _
        '    ByVal PRSN_DataNascita As datetime, _
        '    ByVal PRSN_Cellulare As String, _
        '    ByVal PRSN_Mail As String, _
        '    ByVal PRSN_Login As String, _
        '    ByVal PRSN_fotoPath As String, _
        '    ByVal PRSN_mostraMail As Boolean, _
        '    ByVal PRSN_TPPR_id As Integer, _
        '    ByVal TPPR_descrizione As String, _
        '    ByVal RLPC_id As Integer, _
        '    ByVal RLPC_ultimoCollegamento As datetime, _
        '    ByVal RLPC_attivato As Boolean, _
        '    ByVal RLPC_abilitato As Boolean, _
        '    ByVal RLPC_Responsabile As String, _
        '    ByVal RLPC_IscrittoIl As datetime, _
        '    ByVal RLPC_PRSN_mostraMail As Boolean, _
        '    ByVal TPRL_id As Integer, _
        '    ByVal TPRL_Nome As String, _
        '    ByVal TPRL_gerarchia As Integer, _
        '    ByVal STDN_matricola As String, _
        '    ByVal LKPO_Default As Integer, _
        '    ByVal Totale As Integer)

        '    _RSN_id = RSN_id
        '    _PRSN_Nome = PRSN_Nome
        '    _PRSN_Cognome = PRSN_Cognome
        '    _PRSN_Anagrafica = PRSN_Anagrafica
        '    _PRSN_DataNascita = PRSN_DataNascita
        '    _PRSN_Cellulare = PRSN_Cellulare
        '    _PRSN_Mail = PRSN_Mail
        '    _PRSN_Login = PRSN_Login
        '    _PRSN_fotoPath = PRSN_fotoPath
        '    _PRSN_mostraMail = PRSN_mostraMail
        '    _PRSN_TPPR_id = PRSN_TPPR_id
        '    _TPPR_descrizione = TPPR_descrizione
        '    _RLPC_id = RLPC_id
        '    _RLPC_ultimoCollegamento = RLPC_ultimoCollegamento
        '    _RLPC_attivato = RLPC_attivato
        '    _RLPC_abilitato = RLPC_abilitato
        '    _RLPC_Responsabile = RLPC_Responsabile
        '    _RLPC_IscrittoIl = RLPC_IscrittoIl
        '    _RLPC_PRSN_mostraMail = RLPC_PRSN_mostraMail
        '    _TPRL_id = TPRL_id
        '    _TPRL_Nome = TPRL_Nome
        '    _TPRL_gerarchia = TPRL_gerarchia
        '    _STDN_matricola = STDN_matricola
        '    _LKPO_Default = LKPO_Default
        '    _Totale = Totale
        'End Sub

    End Class
End Namespace