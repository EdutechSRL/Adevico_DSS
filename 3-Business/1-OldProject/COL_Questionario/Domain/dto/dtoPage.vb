<Serializable()>
Public Class dtoPage

#Region "Private"
    Private _Id As Integer
    Private _Number As Integer
    Private _FromQuestion As Integer
    Private _ToQuestion As Integer
    Private _Questions As List(Of dtoPageQuestion)
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
    Public Overridable Property Questions As List(Of dtoPageQuestion)
        Get
            Return _Questions
        End Get
        Set(value As List(Of dtoPageQuestion))
            _Questions = value
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
    Public Overridable Property FromQuestion As Integer
        Get
            Return _FromQuestion
        End Get
        Set(value As Integer)
            _FromQuestion = value
        End Set
    End Property
    Public Overridable Property ToQuestion As Integer
        Get
            Return _ToQuestion
        End Get
        Set(value As Integer)
            _ToQuestion = value
        End Set
    End Property
#End Region




    Sub New()
        _Questions = New List(Of dtoPageQuestion)
    End Sub

End Class