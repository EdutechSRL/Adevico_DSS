<Serializable(), CLSCompliant(True)> Public Class PersonInfo
    Implements IComparable

    Public ID As Integer
    Public Name As String
    Public Surname As String
    Public Mail As String
    Public TaxCode As String
    Public ShowMail As Boolean
    Public Login As String
    Public Password As String
    Public Sex As Integer
    Public PersonTypeID() As Integer
    Public LanguageID As Integer
    Public LanguageName As String
    Public Sub New()

    End Sub
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Dim objPerson As PersonInfo
        objPerson = CType(obj, PersonInfo)
        Me.ID.CompareTo(objPerson.ID)
    End Function
End Class