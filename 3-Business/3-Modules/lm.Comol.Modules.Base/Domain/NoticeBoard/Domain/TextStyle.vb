Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class TextStyle
        Inherits DomainObject(Of Integer)

#Region "Private Property"
        Private _Face As String
        Private _Size As Integer
        Private _Color As String
        Private _Align As String
        Private _BackGround As String
#End Region

#Region "Public Overridable Property"
        Public Overridable Property Face() As String
            Get
                Return _Face
            End Get
            Set(ByVal value As String)
                _Face = value
            End Set
        End Property
        Public Overridable Property Size() As Integer
            Get
                Return _Size
            End Get
            Set(ByVal value As Integer)
                _Size = value
            End Set
        End Property
        Public Overridable Property Color() As String
            Get
                Return _Color
            End Get
            Set(ByVal value As String)
                _Color = value
            End Set
        End Property
        Public Overridable Property Align() As String
            Get
                Return _Align
            End Get
            Set(ByVal value As String)
                _Align = value
            End Set
        End Property
        Public Overridable Property BackGround() As String
            Get
                Return _BackGround
            End Get
            Set(ByVal value As String)
                _BackGround = value
            End Set
        End Property
#End Region
        Public Sub New()
            _Color = ""
            _Align = ""
            _Face = "Verdana"
            _BackGround = "ffffff"
        End Sub
    End Class
End Namespace