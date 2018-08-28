<Serializable()>
Public Class LazyAssociatedQuestion
#Region "Private"
    Private _Id As Integer
    Private _IdQuestionnnaire As Integer
    Private _IdQuestion As Integer
    Private _IdRandomQuestionnnaire As Integer?
    Private _Number As Integer
    Private _Languages As IList(Of LazyMultilanguageQuestion)
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
    Public Overridable Property IdQuestion As Integer
        Get
            Return _IdQuestion
        End Get
        Set(value As Integer)
            _IdQuestion = value
        End Set
    End Property
    Public Overridable Property IdRandomQuestionnnaire As Integer?
        Get
            Return _IdRandomQuestionnnaire
        End Get
        Set(value As Integer?)
            _IdRandomQuestionnnaire = value
        End Set
    End Property
    Public Overridable Property Number As Integer
        Get
            Return _Number
        End Get
        Set(value As Integer)
            _Number = value
        End Set
    End Property
    Public Overridable Property Languages As IList(Of LazyMultilanguageQuestion)
        Get
            Return _Languages
        End Get
        Set(value As IList(Of LazyMultilanguageQuestion))
            _Languages = value
        End Set
    End Property
#End Region
    Public Sub New()
        _Languages = New List(Of LazyMultilanguageQuestion)
    End Sub


End Class