Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookPermission
        Implements iWorkBookPermission

#Region "Private Property"
		Private _AddItems As Boolean
		Private _CreateDiary As Boolean
		Private _ReadDiary As Boolean
		Private _DeleteDiary As Boolean
		Private _ChangeApprovation As Boolean
		Private _ChangeDiary As Boolean
		Private _UndeleteDiary As Boolean
        Private _Admin As Boolean
        Private _ChangeEditing As Boolean
#End Region

#Region "Public Property"
        Public Property ChangeApprovation() As Boolean Implements iWorkBookPermission.ChangeApprovation
            Get
                ChangeApprovation = _ChangeApprovation
            End Get
            Set(ByVal Value As Boolean)
                _ChangeApprovation = Value
            End Set
        End Property
        Public Property DeleteWorkBook() As Boolean Implements iWorkBookPermission.DeleteWorkBook
            Get
                DeleteWorkBook = _DeleteDiary
            End Get
            Set(ByVal Value As Boolean)
                _DeleteDiary = Value
            End Set
        End Property
        Public Property EditWorkBook() As Boolean Implements iWorkBookPermission.EditWorkBook
            Get
                EditWorkBook = _ChangeDiary
            End Get
            Set(ByVal Value As Boolean)
                _ChangeDiary = Value
            End Set
        End Property
        Public Property ReadWorkBook() As Boolean Implements iWorkBookPermission.ReadWorkBook
            Get
                ReadWorkBook = _ReadDiary
            End Get
            Set(ByVal Value As Boolean)
                _ReadDiary = Value
            End Set
        End Property
        Public Property AddItems() As Boolean Implements iWorkBookPermission.AddItems
            Get
                AddItems = _AddItems
            End Get
            Set(ByVal Value As Boolean)
                _AddItems = Value
            End Set
        End Property
        Public Property CreateWorkBook() As Boolean Implements iWorkBookPermission.CreateWorkBook
            Get
                CreateWorkBook = _CreateDiary
            End Get
            Set(ByVal Value As Boolean)
                _CreateDiary = Value
            End Set
        End Property
        Public Property UndeleteWorkBook() As Boolean Implements iWorkBookPermission.UndeleteWorkBook
            Get
                Return _UndeleteDiary
            End Get
            Set(ByVal value As Boolean)
                _UndeleteDiary = value
            End Set
        End Property
		Public Property Admin() As Boolean Implements iWorkBookPermission.Admin
			Get
				Return _Admin
			End Get
			Set(ByVal value As Boolean)
				_Admin = value
			End Set
        End Property
        Public Overridable Property ChangeEditing() As Boolean
            Get
                ChangeEditing = _ChangeEditing
            End Get
            Set(ByVal Value As Boolean)
                _ChangeEditing = Value
            End Set
        End Property
#End Region


    End Class
End Namespace