<Serializable()>
Public Class dtoDisplayName
    Public Property Name As String
    Public Property Surname As String
    Public Property DisplayName As String
    Public Property OtherInfos As String
    Public Property TaxCode As String

    Public Sub New()

    End Sub
    Public Sub New(fullname As String)
        DisplayName = fullname
        Surname = fullname
    End Sub

    'Public Sub New(p As lm.Comol.Core.DomainModel.Person)
    '    Name = p.Name
    '    Surname = p.Surname
    '    DisplayName = p.SurnameAndName
    '    TaxCode = p.TaxCode
    'End Sub
    Public Sub New(p As lm.Comol.Core.DomainModel.litePerson)
        Name = p.Name
        Surname = p.Surname
        DisplayName = p.SurnameAndName
        TaxCode = p.TaxCode
    End Sub
    Public Sub New(p As LazyExternalInvitedUser)
        Name = p.Name
        Surname = p.Surname
        DisplayName = p.Surname & " " & p.Name
        OtherInfos = p.Description
    End Sub

End Class