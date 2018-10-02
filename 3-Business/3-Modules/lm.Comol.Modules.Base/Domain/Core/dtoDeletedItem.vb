Imports lm.Comol.Core.DomainModel.Repository
Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoDeletedItem

#Region "Private Property"
        Private _ID As Long
        Private _Name As String
        Private _isFile As Boolean
        Private _FolderID As Long
        Private _FolderName As String
        Private _Type As RepositoryItemType
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
        Public Property isFile() As Boolean
            Get
                Return _isFile
            End Get
            Set(ByVal value As Boolean)
                _isFile = value
            End Set
        End Property
        Public Property FolderID() As Long
            Get
                Return _FolderID
            End Get
            Set(ByVal value As Long)
                _FolderID = value
            End Set
        End Property
        Public Property FolderName() As String
            Get
                Return _FolderName
            End Get
            Set(ByVal value As String)
                _FolderName = value
            End Set
        End Property
        Public Property Type() As RepositoryItemType
            Get
                Return _Type
            End Get
            Set(ByVal value As RepositoryItemType)
                _Type = value
            End Set
        End Property
#End Region

        Sub New()
            Me._isFile = False
        End Sub
        Sub New(ByVal oId As Long, ByVal oName As String, ByVal oIsFile As Boolean, ByVal oFolderID As Long, ByVal oFolderName As String, ByVal type As RepositoryItemType)
            Me._ID = oId
            Me._Name = oName
            Me._isFile = oIsFile
            Me._FolderID = oFolderID
            Me._FolderName = oFolderName
            Me._Type = type
        End Sub
    End Class
End Namespace