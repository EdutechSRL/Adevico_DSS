Namespace UCServices

    Public Class Services_Neutral
        Inherits Abstract.MyServices

        Private Const Codice As String = "SRVNEUTRAL"

        Public Overloads ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

        Public Shadows Enum PermissionType
            Read = 0
            Write = 1
            Delete = 3
            Moderate = 4
            Grant = 5
            Admin = 6
        End Enum
        Sub New()
            MyBase.New()
        End Sub
        Public Overloads Function GetPermissionValue(ByVal oType As PermissionType) As Boolean
            Return MyBase.GetPermissionByPosition(CType(oType, PermissionType))
        End Function
        Public Overloads Function SetPermissionValue(ByVal oPosizione As Integer, ByVal oValue As Byte) As Boolean
            Return MyBase.SetPermissionByPosition(oPosizione, oValue)
        End Function
        Public Overloads Function GetPermissionByPosition(ByVal oPosizione As Integer) As Boolean
            Return MyBase.GetPermissionByPosition(oPosizione)
        End Function

        'Public Function GetServiceByCode(ByVal Codice)
        '    Dim oService As New Services_CHAT
        '    CBool(oService.Codex = oService.Codex)
        '    Select Case (Codice)
        '        Case Services_CHAT.
        '            Return New Services_CHAT
        '        Case Services_Bacheca.Codice
        '            Return New Services_Bacheca
        '        Case Services_AmministraComunita.Codice
        '            Return New Services_AmministraComunita
        '        Case Services_AmministrazioneGlobale.Codice
        '            Return New Services_AmministrazioneGlobale
        '        Case Services_AmministrazioneSMS.Codice
        '            Return New Services_AmministrazioneSMS
        '        Case Services_Eventi.Codice
        '            Return New Services_Eventi
        '        Case Services_File.Codice
        '            Return New Services_File
        '        Case Services_Forum.Codice
        '            Return New Services_Forum
        '        Case Services_Mail.Codice
        '            Return New Services_Mail
        '        Case Services_PostIt.Codice
        '            Return New Services_PostIt
        '        Case Services_SMS.Codice
        '            Return New Services_SMS
        '        Case Services_Tesi.Codice
        '            Return New Services_Tesi
        '        Case Else
        '            Return Nothing
        '    End Select
        'End Function
    End Class
End Namespace