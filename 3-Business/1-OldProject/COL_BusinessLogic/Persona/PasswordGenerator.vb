Public Class PasswordGenerator
    Private _Length As Integer
    Private _Letters As String
    Private _Numbers As String

    Public Property Length() As Integer
        Get
            Return _Length
        End Get
        Set(ByVal value As Integer)
            _Length = value
        End Set
    End Property
    Public Property Letters() As String
        Get
            Return _Letters
        End Get
        Set(ByVal value As String)
            _Letters = value
        End Set
    End Property
    Public Property Numbers() As String
        Get
            Return _Numbers
        End Get
        Set(ByVal value As String)
            _Numbers = value
        End Set
    End Property

    Public Sub New()
        Length = 8
        letters = "abcdefghijklmnopqrstuvwxyz"
        numbers = "0123456789"
    End Sub

    Public Sub New(ByVal length As Integer, ByVal letters As String, ByVal numbers As String)
        Me.Length = length
        Me.Letters = letters
        Me.Numbers = numbers
    End Sub

    Public Function Encrypt(ByVal clear As String) As String
        'Dim pwdCryptata As New COL_Encrypter
        Dim enc As New COL_Encrypter()
        'COL_Persona Person = new COL_Persona();
        'Person.ModificaPassword(clear);
        'return Person.Pwd;
        Return enc.Encrypt(clear)
    End Function

    Public Function Generate() As String
        Return Generate(Length)
    End Function

    Public Function Generate(ByVal length As Integer) As String
        Return Generate(length, Letters, Numbers)
    End Function

    Public Function Generate(ByVal length As Integer, ByVal letters As String, ByVal numbers As String) As String
        Dim keygen As New RandomKeyGenerator()
        keygen.KeyChars = length
        keygen.KeyLetters = letters
        keygen.KeyNumbers = numbers

        Dim password As String = keygen.Generate()

        Return password
    End Function

    Public Function GenerateEncrypted(ByVal length As Integer, ByVal letters As String, ByVal numbers As String) As String
        Return Encrypt(Generate(length, letters, numbers))
    End Function
    Public Function GenerateEncrypted() As String
        Return Encrypt(Generate(length, letters, numbers))
    End Function
End Class