Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoImportedItem

#Region "Private Property"
        Private _FolderID As Long
        Private _FolderName As String
        Private _FileCount As Long
        Private _FolderCount As Long
#End Region

#Region "Public Property"
        Public Property FileCount() As Long
            Get
                Return _FileCount
            End Get
            Set(ByVal value As Long)
                _FileCount = value
            End Set
        End Property
        Public Property FolderCount() As Long
            Get
                Return _FolderCount
            End Get
            Set(ByVal value As Long)
                _FolderCount = value
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
#End Region

        Sub New()

        End Sub
        Sub New(ByVal oFolderID As Long, ByVal oFolderName As String)
            Me._FolderID = oFolderID
            Me._FolderName = oFolderName
        End Sub
        Sub New(ByVal oFolderID As Long, ByVal oFolderName As String, ByVal oFileCount As Long, ByVal oFolderCount As Long)
            Me._FolderID = oFolderID
            Me._FolderName = oFolderName
            Me._FileCount = oFileCount
            Me._FolderCount = oFolderCount
        End Sub
    End Class
End Namespace