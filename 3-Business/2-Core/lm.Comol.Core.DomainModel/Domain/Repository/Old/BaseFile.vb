Namespace lm.Comol.Core.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class BaseFile
		Inherits lm.Comol.Core.DomainModel.DomainObject(Of System.Guid)
		Implements iBaseFile

#Region "Private"
		Private _Name As String
		Private _Extension As String
		Private _Description As String
		'Private _FileSystemName As String
		Private _Size As Long
		Private _ContentType As String
		Private _MetaData As MetaData
		Private _Owner As Person
		Private _HardLink As Long
		Private _IsDownloadable As Boolean
		Private _IsScormFile As Boolean
#End Region

#Region "Public Overridable Property"
        Public Overridable Property Name() As String Implements iBaseFile.Name
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property
        Public Overridable ReadOnly Property DisplayName() As String Implements iBaseFile.DisplayName
            Get
                Return Me._Name & Me._Extension
            End Get
        End Property
        Public Overridable Property Extension() As String Implements iBaseFile.Extension
            Get
                Return Me._Extension
            End Get
            Set(ByVal value As String)
                Me._Extension = value
            End Set
        End Property
        Public Overridable Property Description() As String Implements iBaseFile.Description
            Get
                Return Me._Description
            End Get
            Set(ByVal value As String)
                Me._Description = value
            End Set
        End Property
        Public Overridable ReadOnly Property FileSystemName() As String Implements iBaseFile.FileSystemName
            Get
                Return MyBase.Id.ToString & ".stored"
            End Get
        End Property
        Public Overridable Property Size() As Long Implements iBaseFile.Size
            Get
                Return Me._Size
            End Get
            Set(ByVal value As Long)
                Me._Size = value
            End Set
        End Property
        Public Overridable Property ContentType() As String Implements iBaseFile.ContentType
            Get
                Return Me._ContentType
            End Get
            Set(ByVal value As String)
                Me._ContentType = value
            End Set
        End Property
        Public Overridable Property MetaInfo() As iMetaData Implements iBaseFile.MetaInfo
            Get
                If IsNothing(Me._MetaData) Then
                    Me._MetaData = New lm.Comol.Core.DomainModel.MetaData
                End If
                Return Me._MetaData
            End Get
            Set(ByVal value As iMetaData)
                Me._MetaData = value
            End Set
        End Property
        Public Overridable Property Owner() As iPerson Implements iBaseFile.Owner
            Get
                Return Me._Owner
            End Get
            Set(ByVal value As iPerson)
                Me._Owner = value
            End Set
        End Property
        Public Overridable Property HardLink() As Long Implements iBaseFile.HardLink
            Get
                Return Me._HardLink
            End Get
            Set(ByVal value As Long)
                Me._HardLink = value
            End Set
        End Property
        Public Overridable ReadOnly Property SizeKB() As Double Implements iBaseFile.SizeKB
            Get
                Return Math.Round(_Size / 1024, 2)
            End Get
        End Property

        Public Overridable ReadOnly Property SizeMB() As Double Implements iBaseFile.SizeMB
            Get
                Return Math.Round(_Size / (1024 * 1024), 2)
            End Get
        End Property
        Public Overridable Property IsDownloadable() As Boolean Implements iBaseFile.IsDownloadable
            Get
                Return _IsDownloadable
            End Get
            Set(ByVal value As Boolean)
                _IsDownloadable = value
            End Set
        End Property
        Public Overridable Property IsScormFile() As Boolean Implements iBaseFile.IsScormFile
            Get
                Return _IsScormFile
            End Get
            Set(ByVal value As Boolean)
                _IsScormFile = value
            End Set
        End Property
#End Region

		Sub New()
			Me._IsScormFile = False
			Me._IsDownloadable = False
		End Sub
	End Class
End Namespace