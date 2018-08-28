Namespace Configuration
    <Serializable(), CLSCompliant(True)> Public Class StyleSettings

#Region "Private properties"
        Private _jqueryMigrateVersion As String
        Private _Menu As String
        Private _Header As String
        Private _jqueryTheme As String
        Private _jqueryVersion As String
        Private _jqueryUIVersion As String
#End Region

#Region "Public properties"
        Public Property Menu() As String
            Get
                Menu = _Menu
            End Get
            Set(ByVal value As String)
                _Menu = value
            End Set
        End Property
        Public Property Header() As String
            Get
                Header = _Header
            End Get
            Set(ByVal value As String)
                _Header = value
            End Set
        End Property
        Public Property jqueryTheme() As String
            Get
                jqueryTheme = _jqueryTheme
            End Get
            Set(ByVal value As String)
                _jqueryTheme = value
            End Set
        End Property
        Public Property jqueryVersion() As String
            Get
                jqueryVersion = _jqueryVersion
            End Get
            Set(ByVal value As String)
                _jqueryVersion = value
            End Set
        End Property
        Public Property jqueryUIVersion() As String
            Get
                jqueryUIVersion = _jqueryUIVersion
            End Get
            Set(ByVal value As String)
                _jqueryUIVersion = value
            End Set
        End Property
        Public Property jqueryMigrateVersion() As String
            Get
                Return _jqueryMigrateVersion
            End Get
            Set(value As String)
                _jqueryMigrateVersion = value
            End Set
        End Property
#End Region

        Public Sub New()
            _Menu = ""
            _jqueryTheme = "Base"
            _jqueryUIVersion = "1.8.13"
            _jqueryVersion = "1.6.1"
        End Sub
    End Class
End Namespace