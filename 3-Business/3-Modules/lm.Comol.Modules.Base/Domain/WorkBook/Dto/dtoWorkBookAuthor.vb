Namespace lm.Comol.Modules.Base.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class dtoWorkBookAuthor

#Region "Private Property"
		Private _Permission As iWorkBookPermission
        Private _Author As WorkBookAuthor
		Private _HeaderTitle As String
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
        Public Property Author() As WorkBookAuthor
            Get
                Return _Author
            End Get
            Set(ByVal value As WorkBookAuthor)
                _Author = value
            End Set
        End Property
		Public Property HeaderTitle() As String
			Get
				Return _HeaderTitle
			End Get
			Set(ByVal value As String)
				_HeaderTitle = value
			End Set
		End Property
#End Region

		Sub New()

		End Sub
        Sub New(ByVal oPermission As WorkBookItemPermission, ByVal oAuthor As WorkBookAuthor, ByVal iHeaderTitle As String)
            Me._Author = oAuthor
            Me._Permission = oPermission
            Me._HeaderTitle = iHeaderTitle
        End Sub

	End Class
End Namespace