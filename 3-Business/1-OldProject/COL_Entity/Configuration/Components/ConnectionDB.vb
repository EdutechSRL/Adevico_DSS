Namespace Configuration.Components
    <Serializable(), CLSCompliant(True)> Public Class ConnectionDB

#Region "Private Property"
        Private _Name As String
        Private _ConnectionString As String
        Private _ProviderName As String
        Private _ConnectionType As ConnectionType
#End Region

#Region "Public Property"
        Public Property Name() As String
            Get
                Name = _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property
        Public Property ConnectionString() As String
            Get
                ConnectionString = _ConnectionString
            End Get
            Set(ByVal Value As String)
                _ConnectionString = Value
            End Set
        End Property
        Public Property ProviderName() As String
            Get
                ProviderName = _ProviderName
            End Get
            Set(ByVal Value As String)
                _ProviderName = Value
            End Set
        End Property
        Public Property DBtype() As ConnectionType
            Get
                DBtype = _ConnectionType
            End Get
            Set(ByVal Value As ConnectionType)
                _ConnectionType = Value
            End Set
        End Property
#End Region

        Sub New(ByVal oType As ConnectionType)
            Me._ConnectionType = oType
        End Sub
        Sub New(ByVal iName As String, ByVal iConnectionString As String, ByVal iProviderName As String, ByVal oType As ConnectionType)
            Me._Name = iName
            Me._ConnectionString = iConnectionString
            Me._ProviderName = iProviderName
            Me._ConnectionType = oType
        End Sub
        Public Shared Function FindByType(ByVal item As ConnectionDB, ByVal argument As ConnectionType) As Boolean
            Return item.DBtype = argument
        End Function
    End Class
End Namespace