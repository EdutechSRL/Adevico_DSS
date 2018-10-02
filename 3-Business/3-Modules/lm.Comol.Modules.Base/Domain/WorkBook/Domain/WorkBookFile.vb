Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookFile
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of System.Guid)

#Region "Private"
        Private _ItemOwner As WorkBookItem
        Private _WorkBookOwner As WorkBook
        Private _Approvation As ApprovationStatus
        Private _Owner As Person
        Private _ApprovedBy As Person
        Private _ApprovedOn As DateTime?
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime?
#End Region

        Public Overridable Property Approvation() As ApprovationStatus
            Get
                Return _Approvation
            End Get
            Set(ByVal value As ApprovationStatus)
                _Approvation = value
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
        Public Overridable Property Owner() As Person
            Get
                Return _Owner
            End Get
            Set(ByVal value As Person)
                _Owner = value
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
        Public Overridable ReadOnly Property isApproved() As Boolean
            Get
                Return (_Approvation = MetaApprovationStatus.Approved Or _Approvation = MetaApprovationStatus.Ignore)
            End Get
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
        Public Overridable Property ItemOwner() As WorkBookItem
            Get
                Return _ItemOwner
            End Get
            Set(ByVal value As WorkBookItem)
                _ItemOwner = value
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

        Sub New()
        End Sub
    End Class
End Namespace