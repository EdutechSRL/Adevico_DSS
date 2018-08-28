<Serializable(), CLSCompliant(True)> Public Class WhiteBoard
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _CommunityOwner As Community
	Private _Text As String
	Private _SpeedStatus As SpeedStatus
	Private _isSimpleVersion As Boolean
	Private _isCurrentVersion As Boolean
	Private _Version As Integer
    Private _Style As TextStyles
#End Region

#Region "Public Property"
    Public Property ID() As Integer
        Get
            ID = _ID
        End Get
        Set(ByVal Value As Integer)
            _ID = Value
        End Set
    End Property
    Public Property CommunityOwner() As Community
        Get
            CommunityOwner = _CommunityOwner
        End Get
        Set(ByVal value As Community)
            _CommunityOwner = value
        End Set
    End Property
    Public Property Text() As String
        Get
            Text = _Text
        End Get
        Set(ByVal Value As String)
            _Text = Value
        End Set
    End Property
    Public Property isCurrentVersion() As Boolean
        Get
            isCurrentVersion = _isCurrentVersion
        End Get
        Set(ByVal Value As Boolean)
            _isCurrentVersion = Value
        End Set
    End Property
    Public Property isSimpleVersion() As Boolean
        Get
            isSimpleVersion = _isSimpleVersion
        End Get
        Set(ByVal Value As Boolean)
            _isSimpleVersion = Value
        End Set
    End Property
    Public Property Speed() As SpeedStatus
        Get
            Speed = _SpeedStatus
        End Get
        Set(ByVal Value As SpeedStatus)
            _SpeedStatus = Value
        End Set
    End Property
    Public Property Style() As TextStyles
        Get
            Style = _Style
        End Get
        Set(ByVal Value As TextStyles)
            _Style = Value
        End Set
    End Property
    Public Property Version() As Integer
        Get
            Version = _Version
        End Get
        Set(ByVal Value As Integer)
            _Version = Value
        End Set
    End Property

    Public ReadOnly Property Visibility() As VisibilityStatus
        Get
            If MyBase.IsDeleted Then
                Return VisibilityStatus.Deleted
            ElseIf MyBase.IsDeleted = False And Me.isCurrentVersion Then
                Return VisibilityStatus.Active
            Else
                Return VisibilityStatus.NotVisible
            End If
        End Get
    End Property
#End Region

    Public Sub New()
        MyBase.IsDeleted = False
        Me._isSimpleVersion = False
        Me._Version = 0
        Me._SpeedStatus = SpeedStatus.Slow
        Me._isCurrentVersion = True
        Me._Style = New TextStyles
    End Sub
    Public Sub New(ByVal CommunityID As Integer, ByVal oCreatore As Person, ByVal isSemplificata As Boolean, Optional ByVal Version As Integer = 0)
        MyBase.IsDeleted = False
        Me._CommunityOwner = New Community(CommunityID)
        MyBase.CreatedBy = oCreatore
        MyBase.ModifiedBy = oCreatore
        Me._isSimpleVersion = isSemplificata
        Me._Version = Version
        Me._SpeedStatus = SpeedStatus.Slow
        Me._isCurrentVersion = True
        Me._Style = New TextStyles
    End Sub
    Public Sub New(ByVal ID As Integer, ByVal oComunita As Community, ByVal oCreatore As Person, ByVal oModificatoDa As Person, ByVal oStile As TextStyles, ByVal isSemplificata As Boolean, _
    ByVal Testo As String, ByVal Speed As SpeedStatus, ByVal isCurrent As Boolean, ByVal isDeleted As Boolean, ByVal Versione As Integer, ByVal CreataIl As DateTime, ByVal ModificataIl As DateTime, ByVal DataCancellazione As DateTime)
        Me._ID = ID
        Me._CommunityOwner = oComunita
        MyBase.CreatedBy = oCreatore
        MyBase.ModifiedBy = oModificatoDa
        Me._Style = oStile
        Me._isSimpleVersion = isSemplificata
        Me._isCurrentVersion = isCurrent
        MyBase.IsDeleted = isDeleted
        Me._Version = Versione
        Me._SpeedStatus = Speed
        MyBase.CreatedAt = CreataIl
        MyBase.ModifiedAt = ModificataIl
        MyBase.DeletedAt = DataCancellazione
        Me._Text = Testo
    End Sub
End Class