Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoFolderFilter

#Region "Private Property"
        Private _ID As Long
        Private _Name As String
        Private _Url As String
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
        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal value As String)
                _Url = value
            End Set
        End Property
#End Region

        Sub New()
        End Sub
        Sub New(ByVal pId As Long, ByVal pName As String)
            Me._ID = pId
            Me._Name = pName
        End Sub
        Sub New(ByVal pId As Long, ByVal pName As String, ByVal pUrl As String)
            Me._ID = pId
            Me._Name = pName
            Me._Url = pUrl
        End Sub
    End Class
End Namespace