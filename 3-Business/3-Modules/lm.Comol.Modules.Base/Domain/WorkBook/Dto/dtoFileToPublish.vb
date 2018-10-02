Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoFileToPublish

#Region "Private Property"
        Private _FileID As System.Guid
        Private _FileName As String
        Private _Extension As String
        Private _CategoryID As Integer
        Private _Visible As Boolean
#End Region

#Region "Public Property"
        Public Property FileID() As System.Guid
            Get
                Return _FileID
            End Get
            Set(ByVal value As System.Guid)
                _FileID = value
            End Set
        End Property
        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal value As String)
                _FileName = value
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
        Public Property CategoryID() As Integer
            Get
                Return _CategoryID
            End Get
            Set(ByVal value As Integer)
                _CategoryID = value
            End Set
        End Property
        Public Property IsVisible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal value As Boolean)
                _Visible = value
            End Set
        End Property
#End Region

    End Class
End Namespace