Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.Presentation

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class NEWdtoWorkbooks

#Region "Private Property"
        Private _Permission As WorkBookPermission
        Private _StatusTranslated As String
        Private _StatusId As Integer
        Private _Editing As EditingPermission
        Private _Title As String
        Private _Body As String
        Private _Authors As List(Of lm.Comol.Core.DomainModel.iPerson)
        Private _Owner As lm.Comol.Core.DomainModel.Person
        Private _IsPersonal As Boolean
        Private _WorkBookType As WorkBookType
        Private _isDraft As Boolean
        Private _ApprovedBy As Person
        Private _ApprovedOn As DateTime?
        Private _CreatedBy As Person
        Private _CreatedOn As DateTime
        Private _isDeleted As Boolean
        Private _ModifiedBy As Person
        Private _ModifiedOn As DateTime?
        Private _ID As System.Guid
        Private _CommunityName As String
        Private _AvailableEditingValues As List(Of EditingSettings)
        Private _AvailableStatusValues As List(Of TranslatedItem(Of Integer))
        Private _isEditable As Boolean
#End Region

#Region "Public Property"
        Public Property ID() As System.Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As System.Guid)
                _ID = value
            End Set
        End Property
        Public Property Permission() As WorkBookPermission
            Get
                Return _Permission
            End Get
            Set(ByVal value As WorkBookPermission)
                _Permission = value
            End Set
        End Property
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

        Public Overridable Property Owner() As Person
            Get
                Return Me._Owner
            End Get
            Set(ByVal value As Person)
                Me._Owner = value
            End Set
        End Property

        Public Overridable Property Authors() As List(Of iPerson)
            Get
                Return Me._Authors
            End Get
            Set(ByVal value As List(Of iPerson))
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
        Public Overridable Property ModifiedOn() As DateTime?
            Get
                Return _ModifiedOn
            End Get
            Set(ByVal value As DateTime?)
                _ModifiedOn = value
            End Set
        End Property
        Public Overridable Property CommunityName() As String
            Get
                Return _CommunityName
            End Get
            Set(ByVal value As String)
                _CommunityName = value
            End Set
        End Property


        Public Overridable Property AvailableEditingValues() As List(Of EditingSettings)
            Get
                Return _AvailableEditingValues
            End Get
            Set(ByVal value As List(Of EditingSettings))
                _AvailableEditingValues = value
            End Set
        End Property
        Public Overridable Property AvailableStatusValues() As List(Of TranslatedItem(Of Integer))
            Get
                Return _AvailableStatusValues
            End Get
            Set(ByVal value As List(Of TranslatedItem(Of Integer)))
                _AvailableStatusValues = value
            End Set
        End Property

        Public Overridable Property isEditable() As Boolean
            Get
                Return _isEditable
            End Get
            Set(ByVal value As Boolean)
                _isEditable = value
            End Set
        End Property

#End Region

        Sub New()
            _AvailableEditingValues = New List(Of EditingSettings)
            _AvailableStatusValues = New List(Of TranslatedItem(Of Integer))
        End Sub
        'Sub New(ByVal oPermission As WorkBookPermission, ByVal oWorkBook As WorkBook, ByVal iStatusTranslated As String, ByVal PortalName As String) 'ByVal iHeaderTitle As String,
        '    ' Me._Item = oItem
        '    Me._Permission = oPermission
        '    'Me._HeaderTitle = iHeaderTitle
        '    Me._StatusTranslated = iStatusTranslated
        '    Me._StatusId = oWorkBook.Status.Id
        '    Me._Editing = oWorkBook.Editing
        '    ModifiedOn = oWorkBook.ModifiedOn
        '    ModifiedBy = oWorkBook.ModifiedBy
        '    isDeleted = oWorkBook.isDeleted
        '    CreatedOn = oWorkBook.CreatedOn
        '    CreatedBy = oWorkBook.CreatedBy
        '    ApprovedOn = oWorkBook.ApprovedOn
        '    ApprovedBy = oWorkBook.ApprovedBy
        '    isDraft = oWorkBook.isDraft
        '    Type = oWorkBook.Type
        '    isPersonal = oWorkBook.isPersonal
        '    Title = oWorkBook.Title
        '    Body = oWorkBook.Text
        '    Title = oWorkBook.Title
        '    Owner = oWorkBook.Owner
        '    Authors = (From wa In oWorkBook.WorkBookAuthors Where wa.IsOwner = False AndAlso wa.MetaInfo.isDeleted = False Select wa.Author).ToList
        '    If oWorkBook.CommunityOwner Is Nothing Then
        '        Me._CommunityName = PortalName
        '    Else
        '        Me._CommunityName = oWorkBook.CommunityOwner.Name
        '    End If
        '    Me.ID = oWorkBook.Id
        'End Sub

        Sub New(ByVal oPermission As WorkBookPermission, ByVal oWorkBook As WorkBook, ByVal iStatusTranslated As String, ByVal PortalName As String, ByVal pAvailableEditingValues As List(Of EditingSettings), ByVal pAvailableStatusValues As List(Of TranslatedItem(Of Integer))) 'ByVal iHeaderTitle As String,
            ' Me._Item = oItem
            Me._Permission = oPermission
            'Me._HeaderTitle = iHeaderTitle
            Me._StatusTranslated = iStatusTranslated
            Me._StatusId = oWorkBook.Status.Id
            Me._Editing = oWorkBook.Editing
            ModifiedOn = oWorkBook.ModifiedOn
            ModifiedBy = oWorkBook.ModifiedBy
            isDeleted = oWorkBook.isDeleted
            CreatedOn = oWorkBook.CreatedOn
            CreatedBy = oWorkBook.CreatedBy
            ApprovedOn = oWorkBook.ApprovedOn
            ApprovedBy = oWorkBook.ApprovedBy
            isDraft = oWorkBook.isDraft
            Type = oWorkBook.Type
            isPersonal = oWorkBook.isPersonal
            Title = oWorkBook.Title
            Body = oWorkBook.Text
            Title = oWorkBook.Title
            Owner = oWorkBook.Owner
            Authors = (From wa In oWorkBook.WorkBookAuthors Where wa.IsOwner = False AndAlso wa.isDeleted = False Select wa.Author).ToList
            If oWorkBook.CommunityOwner Is Nothing Then
                Me._CommunityName = PortalName
            Else
                Me._CommunityName = oWorkBook.CommunityOwner.Name
            End If
            _AvailableEditingValues = pAvailableEditingValues
            _AvailableStatusValues = pAvailableStatusValues
            Me.ID = oWorkBook.Id

            _isEditable = (From p In pAvailableEditingValues Where (p And StatusId) > 0).Count > 0
        End Sub
    

    End Class
End Namespace