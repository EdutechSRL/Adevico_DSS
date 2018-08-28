Imports Comol.Entity.Configuration.Components

Namespace Configuration
    <Serializable(), CLSCompliant(True)> Public Class NotificationSettings

#Region "Private properties"
        Private _Enabled As Boolean
        Private _Services As List(Of ServiceToNotify)
#End Region

#Region "Public properties"
        Public Property Enabled() As Boolean
            Get
                Enabled = _Enabled
            End Get
            Set(ByVal value As Boolean)
                _Enabled = value
            End Set
        End Property
        Public Property Services() As List(Of ServiceToNotify)
            Get
                Services = _Services
            End Get
            Set(ByVal value As List(Of ServiceToNotify))
                _Services = value
            End Set
        End Property
#End Region

        Public Sub New()
            _Enabled = False
            _Services = New List(Of ServiceToNotify)
        End Sub

        Public Function isServiceEnabled(ByVal Code As String) As Boolean
            Return (From oService In _Services Where oService.Code = Code AndAlso oService.Enabled Select oService.Name).Any
        End Function
    End Class
End Namespace