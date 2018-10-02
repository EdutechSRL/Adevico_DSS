Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoGenericFile

#Region "Private Property"
        Private _ID As Long
        Private _Name As String
        Private _Extension As String
        Private _isVisible As Boolean
        Private _Folder As dtoFileFolder
        Private _Selected As Boolean
        Private _Selectable As Boolean
#End Region

#Region "Public Property"
        Public Property ID() As Long
            Get
                Return _ID
            End Get
            Set(ByVal value As Long)
                _ID = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Property Extension() As String
            Get
                Return _Extension
            End Get
            Set(ByVal value As String)
                _Extension = value
            End Set
        End Property
        Public ReadOnly Property DisplayName() As String
            Get
                Return _Name & _Extension
            End Get
        End Property
        Public Property isVisible() As Boolean
            Get
                Return _isVisible
            End Get
            Set(ByVal value As Boolean)
                _isVisible = value
            End Set
        End Property
        'Public Property Folder() As dtoFileFolder
        '    Get
        '        Return _Folder
        '    End Get
        '    Set(ByVal value As dtoFileFolder)
        '        _Folder = value
        '    End Set
        'End Property
        Public Property Selected() As Boolean
            Get
                Return _Selected
            End Get
            Set(ByVal value As Boolean)
                _Selected = value
            End Set
        End Property
        Public Property Selectable() As Boolean
            Get
                Return _Selectable
            End Get
            Set(ByVal value As Boolean)
                _Selectable = value
            End Set
        End Property
#End Region

        Sub New()
            _Selected = False
            _Selectable = True
        End Sub
        Sub New(ByVal oId As Long, ByVal oName As String, ByVal oExtension As String, ByVal oVisible As Boolean)
            Me._ID = oId
            Me._Name = oName
            Me._Extension = oExtension
            Me._isVisible = oVisible
            _Selected = False
            _Selectable = True
        End Sub
        Sub New(ByVal oId As Long, ByVal oName As String, ByVal oExtension As String, ByVal oVisible As Boolean, ByVal oSelected As Boolean)
            Me._ID = oId
            Me._Name = oName
            Me._Extension = oExtension
            Me._isVisible = oVisible
            _Selected = oSelected
            _Selectable = True
        End Sub
        'Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean, ByVal oFolder As dtoFileFolder)
        '    Me._ID = oId
        '    Me._Name = oName
        '    Me._isVisible = oVisible
        '    Me._Folder = oFolder
        'End Sub

    End Class
End Namespace