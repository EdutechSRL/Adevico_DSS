Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookAuthor
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of System.Guid)

#Region "Private Property"
        Private _WorkBook As WorkBook
        Private _Author As lm.Comol.Core.DomainModel.iPerson
        Private _IsOwner As Boolean
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime?
#End Region

#Region "Public Overridable Property"
        Public Overridable Property Author() As Core.DomainModel.iPerson
            Get
                Return _Author
            End Get
            Set(ByVal value As Core.DomainModel.iPerson)
                _Author = value
            End Set
        End Property
        Public Overridable Property WorkBookOwner() As WorkBook
            Get
                Return _WorkBook
            End Get
            Set(ByVal value As WorkBook)
                _WorkBook = value
            End Set
        End Property
        Public Overridable Property IsOwner() As Boolean
            Get
                Return _IsOwner
            End Get
            Set(ByVal value As Boolean)
                _IsOwner = value
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
#End Region

        Sub New()

        End Sub
    End Class
End Namespace