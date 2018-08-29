Public Class TaxCodeGenerator
    Private _Length As Integer
    Private _Letters As String
    Private _Numbers As String

    Public Sub New()
        _Length = 16
        _Letters = "abcdefghijklmnopqrstuvwxyz"
        _Numbers = "0123456789"
    End Sub

    Public Function Generate() As String
        Dim keygen As New RandomKeyGenerator()
        keygen.KeyChars = _Length
        keygen.KeyLetters = _Letters
        keygen.KeyNumbers = _Numbers

        Dim RetrieveCount As Integer = 100
        Dim TaxCode As String = ""
        While RetrieveCount > 0
            TaxCode = keygen.Generate()
            If isTaxCodeValid(TaxCode) = False Then
                RetrieveCount -= 1
            Else
                RetrieveCount = 0
                Exit While
            End If
        End While
        Return TaxCode
    End Function



    Private Function isTaxCodeValid(ByVal TaxCode As String) As Boolean
        Dim oRequest As New COL_Request
        Dim objAccesso As New COL_DataAccess

        Dim iResponse As Boolean = False
        With oRequest
            .Command = "SELECT count(PRSN_ID) as Totale from Persona where PRSN_codFiscale='" & TaxCode & "'"
            .CommandType = CommandType.Text
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Dim iReader As IDataReader = Nothing
        Try
            iReader = objAccesso.GetdataReader(oRequest)
            While iReader.Read
                If iReader.Item("Totale") > 0 Then
                    iResponse = False
                Else
                    iResponse = True
                End If
            End While

        Catch ex As Exception
            iResponse = False
        Finally
            If Not IsNothing(iReader) AndAlso iReader.IsClosed = False Then
                iReader.Close()
                iReader.Dispose()
            End If
        End Try
        Return iResponse
    End Function
End Class