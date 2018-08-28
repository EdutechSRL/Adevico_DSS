Imports lm.Comol.Core.DomainModel

<Serializable()>
Public Class LazyQuestionnaireAssignment
    Inherits DomainBaseObjectMetaInfo(Of Long)

#Region ""
    Private _type As AssignmentType
    Private _quest As LazyQuestionnaire
#End Region

#Region ""
    Public Overridable Property Questionnaire As LazyQuestionnaire
        Get
            Return _quest
        End Get
        Set(value As LazyQuestionnaire)
            _quest = value
        End Set
    End Property
    Public Overridable Property Type As AssignmentType
        Get
            Return _type
        End Get
        Set(value As AssignmentType)
            _type = value
        End Set
    End Property
#End Region

End Class

<Serializable()>
Public Class LazyQuestionnaireCommunityAssignment
    Inherits LazyQuestionnaireAssignment

#Region ""
    Private _Community As Community
#End Region

#Region ""
    Public Overridable Property AssignedTo As Community
        Get
            Return _Community
        End Get
        Set(value As Community)
            _Community = value
        End Set
    End Property
#End Region

    Public Sub New()
        Type = AssignmentType.community
    End Sub
End Class

<Serializable()>
Public Class LazyQuestionnairePersonAssignment
    Inherits LazyQuestionnaireAssignment

#Region ""
    Private _Person As Person
#End Region

#Region ""
    Public Overridable Property AssignedTo As Person
        Get
            Return _Person
        End Get
        Set(value As Person)
            _Person = value
        End Set
    End Property
#End Region
    Public Sub New()
        Type = AssignmentType.person
    End Sub
End Class

<Serializable()>
Public Class LazyQuestionnairePersonTypeAssignment
    Inherits LazyQuestionnaireAssignment

#Region ""
    Private _IdProfileType As Integer
#End Region

#Region ""
    Public Overridable Property AssignedTo As Integer
        Get
            Return _IdProfileType
        End Get
        Set(value As Integer)
            _IdProfileType = value
        End Set
    End Property
#End Region

    Public Sub New()
        Type = AssignmentType.personType
    End Sub

End Class