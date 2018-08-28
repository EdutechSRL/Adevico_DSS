Namespace Configuration.Components
    <Serializable(), CLSCompliant(True)> Public Class ServiceToNotify

#Region "Private properties"
        Private _Code As String
        Private _Enabled As Boolean
        Private _Name As String
#End Region

#Region "Public properties"
        Public Property Code() As String
            Get
                Code = _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property
        Public Property Enabled() As Boolean
            Get
                Enabled = _Enabled
            End Get
            Set(ByVal value As Boolean)
                _Enabled = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Name = _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
#End Region

        Sub New()
            _Enabled = False
        End Sub
    End Class
End Namespace