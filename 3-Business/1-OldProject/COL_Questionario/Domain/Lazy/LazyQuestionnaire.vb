<Serializable()>
Public Class LazyQuestionnaire

#Region "Private"
    Private _Id As Integer
    Private _IdGroup As Integer
    Private _isDisabled As Boolean
    Private _IdType As Integer
    Private _ownerType As Integer
    Private _ownerId As Long
    Private _ownerGUID As Guid
    Private _ForCommunityUsers As Boolean
    Private _ForPortalUsers As Boolean
    Private _ForExternalUsers As Boolean
    Private _ForInvitedUsers As Boolean
    Private _isAnonymous As Boolean
    Private _EvaluationScale As Integer
    Private _MinScore As Integer
    Private _MaxAttempts As Integer
    Private _Accessibility As Integer

    Private _LowQuestionNumber As Integer
    Private _MediumQuestionNumber As Integer
    Private _HighQuestionNumber As Integer
    Private _ViewSuggestions As Boolean
    Private _ViewAnswers As Boolean
    Private _ViewCorrections As Boolean
    Private _EditAnswer As Boolean
    Private _DisplayScoreToUser As Boolean
    Private _DisplayAttemptScoreToUser As Boolean
    Private _DisplayResultsStatus As Boolean
    Private _DisplayAvailableAttempts As Boolean
    Private _DisplayCurrentAttempts As Boolean
    Private _DisplayNotPassedScoreToUser As Boolean
    Private _TimeSavingValidity As Integer?
#End Region

#Region "Public"
    Public Overridable Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property
    Public Overridable Property IdGroup As Integer
        Get
            Return _IdGroup
        End Get
        Set(value As Integer)
            _IdGroup = value
        End Set
    End Property
    Public Overridable Property EvaluationScale As Integer
        Get
            Return _EvaluationScale
        End Get
        Set(value As Integer)
            _EvaluationScale = value
        End Set
    End Property
    Public Overridable Property MinScore As Integer
        Get
            Return _MinScore
        End Get
        Set(value As Integer)
            _MinScore = value
        End Set
    End Property
    Public Overridable Property MaxAttempts As Integer
        Get
            Return _MaxAttempts
        End Get
        Set(value As Integer)
            _MaxAttempts = value
        End Set
    End Property
    Public Overridable Property IdType As Integer
        Get
            Return _IdType
        End Get
        Set(value As Integer)
            _IdType = value
        End Set
    End Property
    Public Overridable Property isDisabled As Boolean
        Get
            Return _isDisabled
        End Get
        Set(value As Boolean)
            _isDisabled = value
        End Set
    End Property
    Public Property OwnerType() As Integer
        Get
            Return _ownerType
        End Get
        Set(ByVal value As Integer)
            _ownerType = value
        End Set
    End Property
    Public Property OwnerId() As Long
        Get
            Return _ownerId
        End Get
        Set(ByVal value As Long)
            _ownerId = value
        End Set
    End Property
    Public Property OwnerGUID() As Guid
        Get
            Return _ownerGUID
        End Get
        Set(ByVal value As Guid)
            _ownerGUID = value
        End Set
    End Property
    Public Overridable Property ForCommunityUsers As Boolean
        Get
            Return _ForCommunityUsers
        End Get
        Set(value As Boolean)
            _ForCommunityUsers = value
        End Set
    End Property
    Public Overridable Property ForPortalUsers As Boolean
        Get
            Return _ForPortalUsers
        End Get
        Set(value As Boolean)
            _ForPortalUsers = value
        End Set
    End Property
    Public Overridable Property ForInvitedUsers As Boolean
        Get
            Return _ForInvitedUsers
        End Get
        Set(value As Boolean)
            _ForInvitedUsers = value
        End Set
    End Property
    Public Overridable Property ForExternalUsers As Boolean
        Get
            Return _ForExternalUsers
        End Get
        Set(value As Boolean)
            _ForExternalUsers = value
        End Set
    End Property
    Public Overridable Property isAnonymous As Boolean
        Get
            Return _isAnonymous
        End Get
        Set(value As Boolean)
            _isAnonymous = value
        End Set
    End Property
    Public Overridable ReadOnly Property Participants As participantType
        Get
            Dim result As participantType = participantType.none
            If ForCommunityUsers Then
                result = result Or participantType.communityUsers
            End If
            If ForExternalUsers Then
                result = result Or participantType.externalUsers
            End If
            If ForInvitedUsers Then
                result = result Or participantType.invitedUsers
            End If
            If ForPortalUsers Then
                result = result Or participantType.allUsers
            End If
            Return result
        End Get
    End Property

    Public Overridable Property LibraryAccessibility As LibraryAccessibility
        Get
            Return _Accessibility
        End Get
        Set(value As LibraryAccessibility)
            _Accessibility = value
        End Set
    End Property

    Public Property HighQuestionNumber() As Integer
        Get
            Return _HighQuestionNumber
        End Get
        Set(ByVal value As Integer)
            _HighQuestionNumber = value
        End Set
    End Property
    Public Property MediumQuestionNumber() As Integer
        Get
            Return _MediumQuestionNumber
        End Get
        Set(ByVal value As Integer)
            _MediumQuestionNumber = value
        End Set
    End Property
    Public Property LowQuestionNumber() As Integer
        Get
            Return _LowQuestionNumber
        End Get
        Set(ByVal value As Integer)
            _LowQuestionNumber = value
        End Set
    End Property
    Public Overridable Property ViewSuggestions As Boolean
        Get
            Return _ViewSuggestions
        End Get
        Set(value As Boolean)
            _ViewSuggestions = value
        End Set
    End Property
    Public Overridable Property ViewAnswers As Boolean
        Get
            Return _ViewAnswers
        End Get
        Set(value As Boolean)
            _ViewAnswers = value
        End Set
    End Property
    Public Overridable Property ViewCorrections As Boolean
        Get
            Return _ViewCorrections
        End Get
        Set(value As Boolean)
            _ViewCorrections = value
        End Set
    End Property
    Public Overridable Property EditAnswer As Boolean
        Get
            Return _EditAnswer
        End Get
        Set(value As Boolean)
            _EditAnswer = value
        End Set
    End Property
    Public Overridable Property DisplayScoreToUser As Boolean
        Get
            Return _DisplayScoreToUser
        End Get
        Set(value As Boolean)
            _DisplayScoreToUser = value
        End Set
    End Property
    Public Overridable Property DisplayAttemptScoreToUser As Boolean
        Get
            Return _DisplayAttemptScoreToUser
        End Get
        Set(value As Boolean)
            _DisplayAttemptScoreToUser = value
        End Set
    End Property
    Public Overridable Property DisplayAvailableAttempts As Boolean
        Get
            Return _DisplayAvailableAttempts
        End Get
        Set(value As Boolean)
            _DisplayAvailableAttempts = value
        End Set
    End Property
    Public Overridable Property DisplayResultsStatus As Boolean
        Get
            Return _DisplayResultsStatus
        End Get
        Set(value As Boolean)
            _DisplayResultsStatus = value
        End Set
    End Property
    Public Overridable Property DisplayCurrentAttempts As Boolean
        Get
            Return _DisplayCurrentAttempts
        End Get
        Set(value As Boolean)
            _DisplayCurrentAttempts = value
        End Set
    End Property
    Public Overridable Property TimeSavingValidity As Integer?
        Get
            Return _TimeSavingValidity
        End Get
        Set(value As Integer?)
            _TimeSavingValidity = value
        End Set
    End Property


#End Region

    Sub New()

    End Sub

End Class