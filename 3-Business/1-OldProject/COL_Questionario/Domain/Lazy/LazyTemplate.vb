Imports lm.Comol.Core.Mail
Imports lm.Comol.Core.MailCommons.Domain.Messages

<Serializable()>
Public Class LazyTemplate

#Region "Private"
    Private _Id As Integer
    Private _IdPerson As Integer
    Private _IdLanguage As Integer

    Private _TemplateType As Integer
    Private _Body As String
    Private _Subject As String
    Private _Name As String
    Private _Link As String
    Private _MailSettings As MessageSettings
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
    Public Overridable Property IdPerson As Integer
        Get
            Return _IdPerson
        End Get
        Set(value As Integer)
            _IdPerson = value
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
    Public Overridable Property TemplateType As Integer
        Get
            Return _TemplateType
        End Get
        Set(value As Integer)
            _TemplateType = value
        End Set
    End Property
    Public Overridable Property Body As String
        Get
            Return _Body
        End Get
        Set(value As String)
            _Body = value
        End Set
    End Property
    Public Overridable Property Subject As String
        Get
            Return _Subject
        End Get
        Set(value As String)
            _Subject = value
        End Set
    End Property
    Public Overridable Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property
    Public Overridable Property Link As String
        Get
            Return _Link
        End Get
        Set(value As String)
            _Link = value
        End Set
    End Property
    Public Overridable Property MailSettings As MessageSettings
        Get
            Return _MailSettings
        End Get
        Set(value As MessageSettings)
            _MailSettings = value
        End Set
    End Property
#End Region

    Sub New()
        _TemplateType = 5
        _MailSettings = New MessageSettings
    End Sub

End Class