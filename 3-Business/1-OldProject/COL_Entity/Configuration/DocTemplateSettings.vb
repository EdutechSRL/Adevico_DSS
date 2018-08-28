Namespace Configuration

    <Serializable(), CLSCompliant(True)> Public Class DocTemplateSettings

        Private _BasePath As String
        Private _BaseTemporaryFolder As String

        Private _BaseUrl As String

        Private _DefaultTemplatePath As String

        Private _FooterFontSize As Integer
        'Private _AvailableServices As IList(Of DTO_Services)


        Public Sub New()
            _BasePath = ""
            _BaseTemporaryFolder = ""

            _BaseUrl = ""
            _DefaultTemplatePath = ""
        End Sub

        ''' <summary>
        ''' Il percorso FISICO in cui sono salvate le immagini relative ai vari loghi.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Usato SIA per il SALVATAGGIO che per attività di modifica/copia.</remarks>
        Public Property BasePath As String
            Get
                Return _BasePath
            End Get
            Set(value As String)
                _BasePath = value
            End Set
        End Property

        ''' <summary>
        ''' La cartella in cui vengono salvati i file temporanei, DENTRO BasePath
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Usato SIA per il SALVATAGGIO che per attività di modifica/copia.</remarks>
        Public Property BaseTemporaryFolder As String
            Get
                Return _BaseTemporaryFolder
            End Get
            Set(value As String)
                _BaseTemporaryFolder = value
            End Set
        End Property


        ''' <summary>
        '''  Il percorso VIRTUALE (WEB) delle immagini dei vari loghi.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Usato sia per le anteprime che per le esportazioni. DEVE essere accessibile via WEB.</remarks>
        Public Property BaseUrl As String
            Get
                Return _BaseUrl
            End Get
            Set(value As String)
                _BaseUrl = value
            End Set
        End Property

        ''' <summary>
        ''' EVENTUALE (implementazione futura) percorso della serializzazione di un template.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' RIVEDERE ventualmente la gestione immagini in questo scenario. (In ogni caso: implementazione futura).
        ''' </remarks>
        Public Property DefaultTemplatePath As String
            Get
                Return _DefaultTemplatePath
            End Get
            Set(value As String)
                _DefaultTemplatePath = value
            End Set
        End Property

        ''' <summary>
        ''' Dimensione del font del footer
        ''' </summary>
        ''' <value>Dimensione font</value>
        ''' <returns>Dimensione font</returns>
        ''' <remarks>
        ''' Se il parametro non è specificato o è = 0, di DEFAULT (hard-coded) è impostato ad 8!
        ''' </remarks>
        Public Property FooterFontSize As Integer
            Get
                Return _FooterFontSize
            End Get
            Set(value As Integer)
                _FooterFontSize = value
            End Set
        End Property
    End Class

    'Public Class DTO_Services
    '    Private _id As Int64
    '    Private _name As String
    '    Public Property Id As Int64
    '        Get
    '            Return _id
    '        End Get
    '        Set(value As Int64)
    '            _id = value
    '        End Set
    '    End Property
    '    Public Property Name As String
    '        Get
    '            Return _name
    '        End Get
    '        Set(value As String)
    '            _name = value
    '        End Set
    '    End Property
    'End Class
End Namespace
