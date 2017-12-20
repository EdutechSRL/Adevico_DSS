'Imports lm.Comol.Core.File
Imports lm.Comol.Core.File
Imports System.Text.RegularExpressions

Public Class SkinUpload

    ''' <summary>
    ''' Dato un UpdateControl ed il nome di destinazione del file, carica il file
    ''' </summary>
    ''' <param name="UpdControl">Il controllo Asp.new Upload</param>
    ''' <param name="FullName">Il nome completo del file da salvare (destinazione)</param>
    ''' <returns>Eventuali errori</returns>
    Public Shared Function UploadFile(ByRef UpdControl As System.Web.UI.WebControls.FileUpload, ByVal FullName As String) As SkUp_ErrorCode

        If Delete.File(FullName) Then
            Dim result As String = Create.UploadFile(UpdControl.PostedFile, FullName)

            Select Case result
                Case FileMessage.FileCreated
                    Return SkUp_ErrorCode.none
                Case FileMessage.NotDeleted
                    Return SkUp_ErrorCode.FileExist
                Case FileMessage.UploadError
                    Return SkUp_ErrorCode.UploadError
                Case Else
                    Return SkUp_ErrorCode.GenericError
            End Select

        Else
            Return SkUp_ErrorCode.FileExist
        End If

    End Function

    ''' <summary>
    ''' Errori di upload
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SkUp_ErrorCode
        none = 0
        FileExist = 1
        UploadError = 2
        GenericError = -1
    End Enum

    Public Shared Function CheckFileName(ByVal CurrentName As String) As String
        Dim Space_Reg As String = "\s"
        Dim Space_Replace As String = "_"

        Dim InvalidChar_Reg As String = "[^.A-Za-z0-9_-]"
        Dim InvalidChar_Replace As String = ""

        Dim rgx As New Regex(Space_Reg)

        CurrentName = rgx.Replace(CurrentName, Space_Replace)

        rgx = New Regex(InvalidChar_Reg)
        CurrentName = rgx.Replace(CurrentName, InvalidChar_Replace)

        Return CurrentName

    End Function
End Class
