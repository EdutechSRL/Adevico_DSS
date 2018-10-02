Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoWorkbooks

#Region "Private Property"
        Private _CommunityID As Integer
        Private _CommunityName As String
        Private _Items As List(Of NEWdtoWorkbooks)

#End Region

#Region "Public Property"
        Public Property CommunityID() As Integer
            Get
                Return _CommunityID
            End Get
            Set(ByVal value As Integer)
                _CommunityID = value
            End Set
        End Property
        Public Property CommunityName() As String
            Get
                Return _CommunityName
            End Get
            Set(ByVal value As String)
                _CommunityName = value
            End Set
        End Property
        Public Property Items() As List(Of NEWdtoWorkbooks)
            Get
                Return _Items
            End Get
            Set(ByVal value As List(Of NEWdtoWorkbooks))
                _Items = value
            End Set
        End Property
        Public ReadOnly Property isPortal() As Boolean
            Get
                Return (_CommunityID <= 0)
            End Get
        End Property
#End Region

        Sub New()
            _Items = New List(Of NEWdtoWorkbooks)
        End Sub
        Sub New(ByVal pID As Integer, ByVal pName As String)
            _CommunityID = pID
            _CommunityName = pName
            _Items = New List(Of NEWdtoWorkbooks)
        End Sub
        'Sub New(ByVal oPermission As WorkBookItemPermission, ByVal oItem As WorkBookItem, ByVal iStatusTranslated As String) 'ByVal iHeaderTitle As String,
        '    Me._Item = oItem
        '    Me._Permission = oPermission
        '    'Me._HeaderTitle = iHeaderTitle
        '    Me._StatusTranslated = iStatusTranslated
        '    Me._StatusId = oItem.Status.Id
        '    Me._Editing = oItem.Editing
        'End Sub
    End Class
End Namespace