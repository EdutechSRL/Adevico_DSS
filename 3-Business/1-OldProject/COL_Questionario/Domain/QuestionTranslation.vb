Public Class QuestionTranslation

#Region "Private"
    Private _Id As Integer
    Private _IdQuestion As Integer
    Private _DisplayText As String
    Private _DisplayBefore As String
    Private _DisplayAfter As String
    Private _Tip As String
    Private _IdLanguage As Integer
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
    Public Overridable Property DisplayText As String
        Get
            Return _DisplayText
        End Get
        Set(value As String)
            _DisplayText = value
        End Set
    End Property
    Public Overridable Property DisplayBefore As String
        Get
            Return _DisplayBefore
        End Get
        Set(value As String)
            _DisplayBefore = value
        End Set
    End Property
    Public Overridable Property DisplayAfter As String
        Get
            Return _DisplayAfter
        End Get
        Set(value As String)
            _DisplayAfter = value
        End Set
    End Property
    Public Overridable Property Tip As String
        Get
            Return _Tip
        End Get
        Set(value As String)
            _Tip = value
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
#End Region

    Sub New()

    End Sub
End Class