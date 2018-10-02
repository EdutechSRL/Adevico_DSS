Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoFileFolder

#Region "Private Property"
        Private _ID As Long
        Private _Name As String
        Private _isVisible As Boolean
        Private _Files As List(Of dtoGenericFile)
        Private _Folder As List(Of dtoFileFolder)
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
        Public Property isVisible() As Boolean
            Get
                Return _isVisible
            End Get
            Set(ByVal value As Boolean)
                _isVisible = value
            End Set
        End Property
        Public Property SubFolders() As List(Of dtoFileFolder)
            Get
                Return _Folder
            End Get
            Set(ByVal value As List(Of dtoFileFolder))
                _Folder = value
            End Set
        End Property
        Public Property Files() As List(Of dtoGenericFile)
            Get
                Return _Files
            End Get
            Set(ByVal value As List(Of dtoGenericFile))
                _Files = value
            End Set
        End Property
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
            Me._Selected = False
            Me._Files = New List(Of dtoGenericFile)
            Me._Folder = New List(Of dtoFileFolder)
        End Sub
        Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean)
            Me._ID = oId
            Me._Name = oName
            Me._isVisible = oVisible
            Me._Selected = False
            Me._Files = New List(Of dtoGenericFile)
            Me._Folder = New List(Of dtoFileFolder)
        End Sub
        Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean, ByVal oSelected As Boolean)
            Me._ID = oId
            Me._Name = oName
            Me._isVisible = oVisible
            Me._Selected = oSelected
            Me._Files = New List(Of dtoGenericFile)
            Me._Folder = New List(Of dtoFileFolder)
        End Sub
    End Class
End Namespace