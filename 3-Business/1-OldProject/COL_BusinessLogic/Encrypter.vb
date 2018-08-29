'Per Compilare sono necessari:
'vbc /t:library /out:..\bin\PWDEncrypter.dll /r:System.dll PWDEncrypter.vb

Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic

Public Class COL_Encrypter
    Enum HashMethod
        MD5
        SHA1
        SHA384
    End Enum

    'Variabili globali della classe
    'NB: le chiavi per la crittazione delle Pwd rimangono sempre uguali, 
    'Altrimenti non mi è possibile controllarle in un altro momento
    Private TdesKey() As Byte = {98, 189, 249, 149, 226, 66, 187, 161, 19, 177, 198, 77, 6, 20, 78, 8, 178, 78, 94, 93, 191, 120, 34, 74}
    Private TdesIV() As Byte = {67, 232, 150, 149, 100, 216, 252, 228}

    'Costruttore della classe
    Public Sub New()
        MyBase.New()
    End Sub

    'function che mi restituisce la stringa in ingresso criptata
    'Secondo le mie chiavi 3des 
    'NB: le chiavi di crittazione rimangono fisse per tutte le crittaz.
    Public Function Encrypt(ByVal InputString As String) As String
        Dim MyMemoryStream As New MemoryStream
        MyMemoryStream.SetLength(0)
        Dim tdes As New TripleDESCryptoServiceProvider
        tdes.Key = TdesKey
        tdes.IV = TdesIV
        Dim encStream As New CryptoStream(MyMemoryStream, tdes.CreateEncryptor(TdesKey, TdesIV), CryptoStreamMode.Write)
        encStream.Write(PlainStringToByteArray(InputString), 0, InputString.Length)
        encStream.Close()
        Return EncryptedByteArrayToString(MyMemoryStream.ToArray)
    End Function

    'function che mi restituisce la stringa in ingresso decriptata
    'Secondo le mie chiavi 3des 
    Public Function Decrypt(ByVal InputString As String) As String

        Dim MyMemoryStream As New MemoryStream
        Dim tdes As New TripleDESCryptoServiceProvider
        tdes.Key = TdesKey
        tdes.IV = TdesIV
        Dim encStream As New CryptoStream(MyMemoryStream, tdes.CreateDecryptor(TdesKey, TdesIV), CryptoStreamMode.Write)
        Dim EncArray() As Byte '(EncryptedStringToByteArray(StringIn).Length) as Byte
        EncArray = EncryptedStringToByteArray(InputString)
        Dim i As Integer = EncArray.Length
        ReDim Preserve EncArray(i - 2)
        encStream.Write(EncArray, 0, EncArray.Length)
        encStream.Close()
        Return PlainByteArrayToString(MyMemoryStream.ToArray)
    End Function


    Public Function Compare(ByVal PlainString As String, ByVal EncryptedString As String) As Boolean
        'function che mi verificadue stringhe, una in chiaro, fornita
        'dall'utente, e una crittata, le confronta e restituisce TRUE se sono
        'identiche, altrimenti FALSE. NB: il confronto viene effettuato sulla
        'stringa crittata 
        If Encrypt(PlainString) = EncryptedString Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function PlainStringToByteArray(ByVal StringIn As String) As Byte()
        Dim i As Integer
        Dim caratteri(StringIn.Length) As Char
        Dim ByteArrayOut(StringIn.Length) As Byte
        caratteri = StringIn.ToCharArray
        For i = 0 To (caratteri.Length - 1)
            ByteArrayOut(i) = CByte(Asc(caratteri(i)))
        Next i
        Return ByteArrayOut
    End Function

    Private Function EncryptedByteArrayToString(ByVal ArrayIn() As Byte) As String
        Dim Caratt As Integer
        Dim StringOut As String = ""
        Dim i As Integer
        Dim NElements As Integer = ArrayIn.Length - 1
        If ArrayIn.Length <> 0 Then
            For i = 0 To NElements
                Caratt = CInt(ArrayIn(i))
                StringOut += Caratt.ToString
                If i <> NElements Then
                    StringOut += "-"
                End If
            Next i

        Else
            StringOut = ""
        End If
        Return StringOut
    End Function

    Private Function EncryptedStringToByteArray(ByVal StringIn As String) As Byte()
        Dim StringaArray() As String
        Dim i, NElementi As Integer
        StringaArray = StringIn.Split("-")
        NElementi = StringaArray.Length
        Dim MyByteArray(NElementi) As Byte
        For i = 0 To (NElementi - 1)
            MyByteArray(i) = CByte(StringaArray(i))
        Next i
        Return MyByteArray
    End Function

    Private Function PlainByteArrayToString(ByVal ArrayIn() As Byte) As String
        Dim Caratt As Char
        Dim StringOut As String = ""
        Dim i As Integer
        Dim NElements As Integer = ArrayIn.Length - 1
        If ArrayIn.Length <> 0 Then
            For i = 0 To NElements
                Caratt = Chr(ArrayIn(i))
                StringOut += Caratt.ToString
            Next i
        Else
            StringOut = ""
        End If
        Return StringOut
    End Function

    Public Shared Function GeneraNumeriCasuali(ByVal MaxChar As Integer) As String
        Dim cifra As String = ""
        Try
            Dim i, codice As Integer
            For i = 1 To MaxChar
                Randomize()
                codice = CInt(Int((9 * Rnd())))
                cifra = cifra & codice
            Next
        Catch ex As Exception
            cifra = "0"
        End Try

        GeneraNumeriCasuali = cifra
    End Function

    Public Shared Function GenerateHashDigest(ByVal source As String, ByVal algorithm As HashMethod) As String
        Dim hashAlgorithm As HashAlgorithm = Nothing
        Select Case algorithm
            Case HashMethod.MD5
                hashAlgorithm = New MD5CryptoServiceProvider
            Case HashMethod.SHA1
                hashAlgorithm = New SHA1CryptoServiceProvider
            Case HashMethod.SHA384
                hashAlgorithm = New SHA384Managed
            Case Else
                ' Error case.
        End Select

        Dim byteValue() As Byte = Encoding.UTF8.GetBytes(source)
        Dim hashValue() As Byte = hashAlgorithm.ComputeHash(byteValue)
        Return Convert.ToBase64String(hashValue)
    End Function

End Class