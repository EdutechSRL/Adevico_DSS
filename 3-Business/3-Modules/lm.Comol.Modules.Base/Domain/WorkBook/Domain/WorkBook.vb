Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBook
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of System.Guid)

#Region "Private Property"
        Private _Title As String
        Private _Text As String
        Private _Note As String
        Private _Items As IList(Of WorkBookItem)
		Private _CommunityOwner As lm.Comol.Core.DomainModel.Community
        Private _Authors As IList(Of WorkBookAuthor)
		Private _Owner As lm.Comol.Core.DomainModel.Person
        'Private _MetaInfo As lm.Comol.Core.DomainModel.MetaData
		Private _IsPersonal As Boolean
        Private _WorkBookType As WorkBookType

        Private _isDraft As Boolean

        Private _Status As WorkBookStatus
        Private _Editing As EditingPermission

        Private _ApprovedBy As Person
        Private _ApprovedOn As DateTime?
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime
#End Region

#Region "Public Overridable Property"
        Public Overridable Property Title() As String
            Get
                Return Me._Title
            End Get
            Set(ByVal value As String)
                Me._Title = value
            End Set
        End Property
        Public Overridable Property Text() As String
            Get
                Return Me._Text
            End Get
            Set(ByVal value As String)
                Me._Text = value
            End Set
        End Property
        Public Overridable Property Note() As String
            Get
                Return Me._Note
            End Get
            Set(ByVal value As String)
                Me._Note = value
            End Set
        End Property
        Public Overridable Property Items() As IList(Of WorkBookItem)
            Get
                Return Me._Items
            End Get
            Set(ByVal value As IList(Of WorkBookItem))
                Me._Items = value
            End Set
        End Property
        Public Overridable Property CommunityOwner() As iCommunity
            Get
                Return Me._CommunityOwner
            End Get
            Set(ByVal value As iCommunity)
                Me._CommunityOwner = value
            End Set
        End Property
        'Public Overridable Property MetaInfo() As lm.Comol.Core.DomainModel.iMetaData
        '    Get
        '        If IsNothing(Me._MetaInfo) Then
        '            Me._MetaInfo = New lm.Comol.Core.DomainModel.MetaData
        '        End If
        '        Return Me._MetaInfo
        '    End Get
        '    Set(ByVal value As lm.Comol.Core.DomainModel.iMetaData)
        '        Me._MetaInfo = value
        '    End Set
        'End Property
        Public Overridable Property Owner() As lm.Comol.Core.DomainModel.iPerson
            Get
                Return Me._Owner
            End Get
            Set(ByVal value As lm.Comol.Core.DomainModel.iPerson)
                Me._Owner = value
            End Set
        End Property
        Public Overridable ReadOnly Property Authors(Optional ByVal Status As ObjectStatus = ObjectStatus.Active) As IList(Of Core.DomainModel.iPerson)
            Get
                If IsNothing(Me._Authors) Then
                    Return New List(Of iPerson)
                Else
                    Select Case Status
                        Case ObjectStatus.All
                            Return (From o In Me._Authors Select o.Author).ToList
                        Case ObjectStatus.Active
                            Return (From o In Me._Authors Where Not o.isDeleted Select o.Author).ToList
                        Case ObjectStatus.Deleted
                            Return (From o In Me._Authors Where o.isDeleted Select o.Author).ToList
                        Case Else
                            Return (From o In Me._Authors Select o.Author).ToList
                    End Select
                End If
            End Get
        End Property
        Public Overridable Property WorkBookAuthors() As IList(Of WorkBookAuthor)
            Get
                Return Me._Authors
            End Get
            Set(ByVal value As IList(Of WorkBookAuthor))
                Me._Authors = value
            End Set
        End Property
        Public Overridable Property isPersonal() As Boolean
            Get
                Return Me._IsPersonal
            End Get
            Set(ByVal value As Boolean)
                Me._IsPersonal = value
            End Set
        End Property

        Public Overridable Property Type() As WorkBookType
            Get
                Return _WorkBookType
            End Get
            Set(ByVal value As WorkBookType)
                _WorkBookType = value
            End Set
        End Property


        Public Overridable Property isDraft() As Boolean
            Get
                Return Me._isDraft
            End Get
            Set(ByVal value As Boolean)
                Me._isDraft = value
            End Set
        End Property


        Public Overridable Property ApprovedBy() As Person
            Get
                Return _ApprovedBy
            End Get
            Set(ByVal value As Person)
                _ApprovedBy = value
            End Set
        End Property
        Public Overridable Property ApprovedOn() As DateTime?
            Get
                Return _ApprovedOn
            End Get
            Set(ByVal value As Date?)
                _ApprovedOn = value
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
        Public Overridable Property ModifiedOn() As DateTime
            Get
                Return _ModifiedOn
            End Get
            Set(ByVal value As DateTime)
                _ModifiedOn = value
            End Set
        End Property
        Public Overridable Property Status() As WorkBookStatus
            Get
                Return _Status
            End Get
            Set(ByVal value As WorkBookStatus)
                _Status = value
            End Set
        End Property
        Public Overridable Property Editing() As EditingPermission
            Get
                Return _Editing
            End Get
            Set(ByVal value As EditingPermission)
                _Editing = value
            End Set
        End Property
#End Region

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Dim other As WorkBook = CType(obj, WorkBook)
            Return Me.Id.Equals(other.Id)
        End Function

        Sub New()
            _Note = ""
            _Text = ""
            _Title = ""
            '  _WorkBookType = WorkBookType.Personal
            _Editing = EditingPermission.Responsible OrElse EditingPermission.Owner OrElse EditingPermission.ModuleManager
        End Sub
    End Class
End Namespace