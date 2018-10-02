Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class NoticeBoard
        Inherits DomainObject(Of Long)

#Region "Private Property"
        Private _Style As TextStyle
        Private _CommunityOwner As Community
        Private _Owner As Person
        Private _CreateByAdvancedEditor As Boolean
        Private _Message As String
        Private _isForPortal As Boolean
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime?
#End Region

#Region "Public Property"
        Public Overridable Property CommunityOwner() As Community
            Get
                Return Me._CommunityOwner
            End Get
            Set(ByVal value As Community)
                Me._CommunityOwner = value
            End Set
        End Property
        Public Overridable Property Owner() As Person
            Get
                Return Me._Owner
            End Get
            Set(ByVal value As Person)
                Me._Owner = value
            End Set
        End Property
        Public Overridable Property Message() As String
            Get
                Message = _Message
            End Get
            Set(ByVal Value As String)
                _Message = Value
            End Set
        End Property
        Public Overridable Property CreateByAdvancedEditor() As Boolean
            Get
                CreateByAdvancedEditor = _CreateByAdvancedEditor
            End Get
            Set(ByVal Value As Boolean)
                _CreateByAdvancedEditor = Value
            End Set
        End Property

        Public Overridable Property isForPortal() As Boolean
            Get
                isForPortal = _isForPortal
            End Get
            Set(ByVal Value As Boolean)
                _isForPortal = Value
            End Set
        End Property

        Public Overridable Property Style() As TextStyle
            Get
                Style = _Style
            End Get
            Set(ByVal Value As TextStyle)
                _Style = Value
            End Set
        End Property
        Public Overridable Property CreatedBy() As Person
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As Person)
                _CreatedBy = value
            End Set
        End Property
        Public Overridable Property CreatedOn() As DateTime?
            Get
                Return _CreatedOn
            End Get
            Set(ByVal value As DateTime?)
                _CreatedOn = value
            End Set
        End Property
        Public Overridable Property isDeleted() As Boolean
            Get
                Return _isDeleted
            End Get
            Set(ByVal value As Boolean)
                _isDeleted = value
            End Set
        End Property
        Public Overridable Property ModifiedBy() As Person
            Get
                Return _ModifiedBy
            End Get
            Set(ByVal value As Person)
                _ModifiedBy = value
            End Set
        End Property
        Public Overridable Property ModifiedOn() As DateTime?
            Get
                Return _ModifiedOn
            End Get
            Set(ByVal value As DateTime?)
                _ModifiedOn = value
            End Set
        End Property
#End Region
        Public Sub New()
            _Message = ""
            _CreateByAdvancedEditor = True
            _isForPortal = False
        End Sub
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Dim other As NoticeBoard = CType(obj, NoticeBoard)
            Return Me.Id.Equals(other.Id) AndAlso Me.isForPortal.Equals(other.isForPortal)
        End Function
    End Class
End Namespace