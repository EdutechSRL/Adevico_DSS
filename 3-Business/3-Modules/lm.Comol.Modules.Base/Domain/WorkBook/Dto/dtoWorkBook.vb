Namespace lm.Comol.Modules.Base.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class dtoWorkBook

#Region "Private Property"
        Private _Permission As iWorkBookPermission
        Private _WorkBook As WorkBook
#End Region

#Region "Public Property"
        Public Property Permission() As iWorkBookPermission
            Get
                Return _Permission
            End Get
            Set(ByVal value As iWorkBookPermission)
                _Permission = value
            End Set
        End Property
        Public Property WorkBook() As WorkBook
            Get
                Return _WorkBook
            End Get
            Set(ByVal value As WorkBook)
                _WorkBook = value
            End Set
        End Property
#End Region

        Sub New()

        End Sub
        Sub New(ByVal oPermission As iWorkBookPermission, ByVal oDiary As WorkBook)
            Me._WorkBook = oDiary
            Me._Permission = oPermission
        End Sub

	End Class
End Namespace