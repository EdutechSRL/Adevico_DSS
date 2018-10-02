Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookItem
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of System.Guid)

#Region "Private Property"
        Private _Title As String
        Private _Body As String
        Private _Note As String
        Private _StartDate As DateTime?
        Private _EndDate As DateTime?
        'Private _MetaInfo As lm.Comol.Core.DomainModel.MetaData
        Private _WorkBookOwner As WorkBook
        Private _Files As IList(Of WorkBookFile)
        Private _Owner As lm.Comol.Core.DomainModel.Person
        Private _isDraft As Boolean

        Private _Status As WorkBookStatus
        Private _Editing As EditingPermission

        Private _ApprovedBy As Person
        Private _ApprovedOn As DateTime?
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime?
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
        Public Overridable Property Body() As String
            Get
                Return Me._Body
            End Get
            Set(ByVal value As String)
                Me._Body = value
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
        Public Overridable Property StartDate() As DateTime
            Get
                Return Me._StartDate
            End Get
            Set(ByVal value As DateTime)
                Me._StartDate = value
            End Set
        End Property
        Public Overridable Property EndDate() As DateTime
            Get
                Return Me._EndDate
            End Get
            Set(ByVal value As DateTime)
                Me._EndDate = value
            End Set
        End Property
        Public Overridable Property WorkBookOwner() As WorkBook
            Get
                Return _WorkBookOwner
            End Get
            Set(ByVal value As WorkBook)
                _WorkBookOwner = value
            End Set
        End Property
        Public Overridable Property Files() As IList(Of WorkBookFile)
            Get
                Return _Files
            End Get
            Set(ByVal value As IList(Of WorkBookFile))
                Me._Files = value
            End Set
        End Property
        Public Overridable Property Owner() As lm.Comol.Core.DomainModel.iPerson
            Get
                Return Me._Owner
            End Get
            Set(ByVal value As lm.Comol.Core.DomainModel.iPerson)
                Me._Owner = value
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
        Public Overridable Property ModifiedOn() As DateTime?
            Get
                Return _ModifiedOn
            End Get
            Set(ByVal value As DateTime?)
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

        Sub New()
            _Note = ""
            _Body = ""
            _Title = ""
            _Editing = EditingPermission.Responsible OrElse EditingPermission.Authors OrElse EditingPermission.Owner
        End Sub
    End Class
End Namespace