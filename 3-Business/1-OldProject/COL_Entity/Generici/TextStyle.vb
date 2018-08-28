<Serializable(), CLSCompliant(True)> Public Class TextStyles

#Region "Private Property"
    Private _face As String
    Private _size As Integer
    Private _color As String
    Private _align As String
    Private _BackGround As String
#End Region

#Region "Public Property"
    Public Property Face() As String
        Get
            Face = _face
        End Get
        Set(ByVal Value As String)
            _face = Value
        End Set
    End Property
    Public Property Size() As Integer
        Get
            Size = _size
        End Get
        Set(ByVal Value As Integer)
            _size = Value
        End Set
    End Property
    Public Property Color() As String
        Get
            Color = _color
        End Get
        Set(ByVal Value As String)
            _color = Value
        End Set
    End Property
    Public Property Align() As String
        Get
            Align = _align
        End Get
        Set(ByVal Value As String)
            _align = Value
        End Set
    End Property
    Public Property BackGround() As String
        Get
            BackGround = _BackGround
        End Get
        Set(ByVal Value As String)
            _BackGround = Value
        End Set
    End Property
#End Region

    Public Sub New()

    End Sub

    Public Function isDefined() As Boolean
        If Me._align = "" AndAlso (_size = 3 Or _size = 0) AndAlso _color = "" AndAlso _align = "" AndAlso _BackGround = "" Then
            Return False
        Else
            Return True
        End If
    End Function
End Class