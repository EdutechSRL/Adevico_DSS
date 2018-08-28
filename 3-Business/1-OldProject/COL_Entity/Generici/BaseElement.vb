<Serializable(), CLSCompliant(True)> Public Class BaseElement
    Private _ID As Integer
    Private _isSelected As Boolean

    Public Property id() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property isSelected() As Boolean
        Get
            Return _isSelected
        End Get
        Set(ByVal value As Boolean)
            _isSelected = value
        End Set
    End Property
End Class