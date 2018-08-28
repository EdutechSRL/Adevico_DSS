<Serializable()>
Public Class LazyMultilanguageQuestion

#Region "Private"
    Private _Id As Integer
    Private _IdLanguage As Integer
    Private _IdQuestion As Integer
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
    Public Overridable Property IdLanguage As Integer
        Get
            Return _IdLanguage
        End Get
        Set(value As Integer)
            _IdLanguage = value
        End Set
    End Property
    Public Property IdQuestion() As Integer
        Get
            Return _IdQuestion
        End Get
        Set(ByVal value As Integer)
            _IdQuestion = value
        End Set
    End Property
#End Region

    Sub New()

    End Sub

End Class