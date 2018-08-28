Namespace Configuration

    <Serializable(), CLSCompliant(True)> Public Class SkinSettings

        Private _PersonsTypeIds As List(Of Integer)
        Private _PersonsIds As List(Of Integer)
        Private _HeadLogo As Logo
        Private _FootText As String
        Private _FootLogos As IList(Of Logo)
        Private _MainCss As String
        Private _LoginCss As String
        Private _AdminCss As String
        Private _IECss As String

        Private _SkinPhisicalPath As String
        Private _SkinVirtualPath As String

        Private _LoginInfos As IList(Of String)

        Private _PortalSetting As PortalOrganizationElements


        Public Sub New()
            _HeadLogo = New Logo
            _FootText = ""
            _FootLogos = New List(Of Logo)
            _MainCss = ""
            _LoginCss = ""
            _AdminCss = ""
            _IECss = ""

            _SkinPhisicalPath = ""
            _SkinVirtualPath = ""
            _LoginInfos = New List(Of String)
            _PersonsTypeIds = New List(Of Integer)
            _PersonsIds = New List(Of Integer)

            _PortalSetting = PortalOrganizationElements.Footer And PortalOrganizationElements.MainLogo
        End Sub

        Public Property PersonTypeIds As List(Of Integer)
            Get
                Return _PersonsTypeIds
            End Get
            Set(ByVal value As List(Of Integer))
                _PersonsTypeIds = value
            End Set
        End Property
        Public Property PersonsIds As List(Of Integer)
            Get
                Return _PersonsIds
            End Get
            Set(ByVal value As List(Of Integer))
                _PersonsIds = value
            End Set
        End Property

        Public Property HeadLogo As Logo
            Get
                Return _HeadLogo
            End Get
            Set(value As Logo)
                _HeadLogo = value
            End Set
        End Property

        Public Property FootText As String
            Get
                Return _FootText
            End Get
            Set(value As String)
                _FootText = value
            End Set
        End Property
        Public Property FootLogos As IList(Of Logo)
            Get
                If IsNothing(_FootLogos) Then
                    _FootLogos = New List(Of Logo)
                End If

                Return _FootLogos
            End Get
            Set(value As IList(Of Logo))
                _FootLogos = value
            End Set
        End Property

        Public Property SkinPhisicalPath() As String
            Get
                Return _SkinPhisicalPath
            End Get
            Set(ByVal value As String)
                If Not (value.EndsWith("\")) Then
                    value &= "\"
                End If

                _SkinPhisicalPath = value
            End Set
        End Property
        Public Property SkinVirtualPath() As String
            Get
                Return _SkinVirtualPath
            End Get
            Set(ByVal value As String)
                _SkinVirtualPath = value
            End Set
        End Property

        Public Class Logo
            Private _Url As String
            Private _Alt As String
            Private _Link As String

            Public Sub New()
                _Url = ""
                _Alt = ""
                _Link = ""
            End Sub

            Public Property Url As String
                Get
                    Return _Url
                End Get
                Set(value As String)
                    _Url = value
                End Set
            End Property

            Public Property Alt As String
                Get
                    Return _Alt
                End Get
                Set(value As String)
                    _Alt = value
                End Set
            End Property
            Public Property Link As String
                Get
                    Return _Link
                End Get
                Set(value As String)
                    _Link = value
                End Set
            End Property
        End Class

        Public Property MainCss As String
            Get
                Return _MainCss
            End Get
            Set(value As String)
                _MainCss = value
            End Set
        End Property
        Public Property LoginCss As String
            Get
                Return _LoginCss
            End Get
            Set(value As String)
                _LoginCss = value
            End Set
        End Property
        Public Property IECss As String
            Get
                Return _IECss
            End Get
            Set(value As String)
                _IECss = value
            End Set
        End Property
        Public Property AdminCss As String
            Get
                Return _AdminCss
            End Get
            Set(value As String)
                _AdminCss = value
            End Set
        End Property

        Public Property LoginInfos As IList(Of String)
            Get
                Return _LoginInfos
            End Get
            Set(value As IList(Of String))
                _LoginInfos = value
            End Set
        End Property
        Public Property TranslatedLoginInfos As List(Of IDictionary())
            Get
                Return _LoginInfos
            End Get
            Set(value As List(Of IDictionary()))
                _LoginInfos = value
            End Set
        End Property

        Public Property PortalSetting As PortalOrganizationElements
            Get
                Return _PortalSetting
            End Get
            Set(value As PortalOrganizationElements)
                _PortalSetting = value
            End Set
        End Property

        ''' <summary>
        ''' Comportamento delle skin a livello di PORTALE
        ''' </summary>
        ''' <remarks></remarks>
        <Serializable(), CLSCompliant(True), Flags> _
        Public Enum PortalOrganizationElements As Integer
            ''' <summary>
            ''' Usa le impostazioni di portale
            ''' </summary>
            ''' <remarks></remarks>
            None = 0
            ''' <summary>
            ''' Usa i CSS dell'organizzazione di DEFAULT dell'utente
            ''' </summary>
            ''' <remarks></remarks>
            Css = 1
            ''' <summary>
            ''' Usa il LOGO dell'organizzazione di DEFAULT dell'utente
            ''' </summary>
            ''' <remarks></remarks>
            MainLogo = 2
            ''' <summary>
            ''' Usa il Footer dell'organizzazione di DEFAULT dell'utente
            ''' </summary>
            ''' <remarks></remarks>
            Footer = 4
            ''' <summary>
            ''' Usa tutte le impostazioni dell'Organizzazione di DEFAULT dell'utente
            ''' </summary>
            ''' <remarks></remarks>
            ALL = 7
        End Enum

    End Class

End Namespace