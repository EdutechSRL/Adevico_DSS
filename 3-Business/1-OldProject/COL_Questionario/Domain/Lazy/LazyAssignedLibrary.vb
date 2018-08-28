<Serializable()>
Public Class LazyAssignedLibrary

#Region "Private"
    Private _Id As Integer
    Private _IdQuestionnnaire As Integer
    Private _IdLibrary As Integer
    Private _EasyQuestionToUse As Integer
    Private _MediumQuestionToUse As Integer
    Private _HighQuestionToUse As Integer
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
    Public Overridable Property IdQuestionnnaire As Integer
        Get
            Return _IdQuestionnnaire
        End Get
        Set(value As Integer)
            _IdQuestionnnaire = value
        End Set
    End Property
    Public Overridable Property IdLibrary As Integer
        Get
            Return _IdLibrary
        End Get
        Set(value As Integer)
            _IdLibrary = value
        End Set
    End Property
    Public Overridable Property EasyQuestionToUse As Integer
        Get
            Return _EasyQuestionToUse
        End Get
        Set(value As Integer)
            _EasyQuestionToUse = value
        End Set
    End Property
    Public Overridable Property MediumQuestionToUse As Integer
        Get
            Return _MediumQuestionToUse
        End Get
        Set(value As Integer)
            _MediumQuestionToUse = value
        End Set
    End Property
    Public Overridable Property HighQuestionToUse As Integer
        Get
            Return _HighQuestionToUse
        End Get
        Set(value As Integer)
            _HighQuestionToUse = value
        End Set
    End Property
#End Region

End Class