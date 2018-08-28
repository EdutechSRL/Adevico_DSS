<Serializable()>
Public Class dtoPageQuestion

#Region "Private"
    Private _Id As Integer
    Private _IdQuestion As Integer
    Private _Number As Integer
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
    Public Overridable Property IdQuestion As Integer
        Get
            Return _IdQuestion
        End Get
        Set(value As Integer)
            _IdQuestion = value
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
#End Region

    Sub New()

    End Sub

End Class