Namespace lm.Comol.Modules.Base.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class dtoWorkBookItem

#Region "Private Property"
        Private _Permission As WorkBookItemPermission
        Private _Item As WorkBookItem
        'Private _HeaderTitle As String
        Private _StatusTranslated As String
        Private _StatusId As Integer
        Private _Editing As EditingPermission
#End Region

#Region "Public Property"
        Public Property Permission() As WorkBookItemPermission
            Get
                Return _Permission
            End Get
            Set(ByVal value As WorkBookItemPermission)
                _Permission = value
            End Set
        End Property
        Public Property Item() As WorkBookItem
            Get
                Return _Item
            End Get
            Set(ByVal value As WorkBookItem)
                _Item = value
            End Set
        End Property
        'Public Property HeaderTitle() As String
        '    Get
        '        Return _HeaderTitle
        '    End Get
        '    Set(ByVal value As String)
        '        _HeaderTitle = value
        '    End Set
        'End Property
        Public Property StatusId() As Integer
            Get
                Return _StatusId
            End Get
            Set(ByVal value As Integer)
                _StatusId = value
            End Set
        End Property
        Public Property StatusTranslated() As String
            Get
                Return _StatusTranslated
            End Get
            Set(ByVal value As String)
                _StatusTranslated = value
            End Set
        End Property
        Public Property Editing() As EditingPermission
            Get
                Return _Editing
            End Get
            Set(ByVal value As EditingPermission)
                _Editing = value
            End Set
        End Property
#End Region

		Sub New()

		End Sub
        Sub New(ByVal oPermission As WorkBookItemPermission, ByVal oItem As WorkBookItem, ByVal iStatusTranslated As String) 'ByVal iHeaderTitle As String,
            Me._Item = oItem
            Me._Permission = oPermission
            'Me._HeaderTitle = iHeaderTitle
            Me._StatusTranslated = iStatusTranslated
            Me._StatusId = oItem.Status.Id
            Me._Editing = oItem.Editing
        End Sub

	End Class
End Namespace