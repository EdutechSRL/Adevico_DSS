Imports System.Configuration
Imports COL_DataLayer
Imports COL_BusinessLogic_v2.CL_persona

Public Module MainNotifica
    Public Function HasConnessioniDB() As Boolean
        Dim oRequest As New COL_Request
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_HasConnessioniDB"
            .CommandType = CommandType.StoredProcedure

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            Return objAccesso.HasDBConnection(oRequest)

        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function


    Public Function LinkRimuoviNotifica(ByVal PRSN_Id As Integer, ByVal Percorso As String, ByVal ForumId As Integer, ByVal TopicID As Integer, Optional ByVal PostID As Integer = 0) As String
        Dim oStringaSelezione As Main.FiltroSelezioneForum
        Dim Link As String = ""
        Try
            If ForumId > 0 And TopicID > 0 And PostID > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost
            ElseIf ForumId > 0 And TopicID > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic
            ElseIf ForumId > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.Forum
            Else
                Return ""
            End If

            Dim RandomCode As String
            RandomCode = COL_Persona.generaPasswordNumerica(5)
			Link = Percorso & "?AddCode=" & PRSN_Id & RandomCode

            If oStringaSelezione = Main.FiltroSelezioneForum.Forum Then
                RandomCode = COL_Persona.generaPasswordNumerica(4)
                Link = Link & "&ExpUrl=k" & RandomCode & ForumId
            ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic Then
                RandomCode = COL_Persona.generaPasswordNumerica(5)
                Link = Link & "&ExpUrl2=j" & RandomCode & TopicID
            ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
                RandomCode = COL_Persona.generaPasswordNumerica(8)
                Link = Link & "&for=x25jt" & RandomCode & PostID
            End If
        Catch ex As Exception

        End Try


        Return Link

    End Function
    ' Public Function LinkRimuoviNotificaArea(ByVal PRSN_Id As Integer, ByVal Percorso As String, ByVal ForumId As Integer, ByVal TopicID As Integer, Optional ByVal PostID As Integer = 0) As String
    '     Dim oStringaSelezione As Main.FiltroSelezioneForum
    '     Dim Link As String = ""
    '     Try
    '         If ForumId > 0 And TopicID > 0 And PostID > 0 Then
    '             oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost
    '         ElseIf ForumId > 0 And TopicID > 0 Then
    '             oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic
    '         ElseIf ForumId > 0 Then
    '             oStringaSelezione = Main.FiltroSelezioneForum.Forum
    '         Else
    '             Return ""
    '         End If

    '         Dim RandomCode As String
    '         RandomCode = COL_Persona.generaPasswordNumerica(5)
    'Link = "http://" & Percorso & "/RimuoviNotificaArea.aspx?AddCode=" & PRSN_Id & RandomCode

    '         If oStringaSelezione = Main.FiltroSelezioneForum.Forum Then
    '             RandomCode = COL_Persona.generaPasswordNumerica(4)
    '             Link = Link & "&ExpUrl=k" & RandomCode & ForumId
    '         ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic Then
    '             RandomCode = COL_Persona.generaPasswordNumerica(5)
    '             Link = Link & "&ExpUrl2=j" & RandomCode & TopicID
    '         ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
    '             RandomCode = COL_Persona.generaPasswordNumerica(8)
    '             Link = Link & "&for=x25jt" & RandomCode & PostID
    '         End If
    '     Catch ex As Exception

    '     End Try


    '     Return Link

    ' End Function
    Public Function LinkAccessoForum(ByVal idPerson As Integer, ByVal CMNT_ID As Integer, ByVal Percorso As String, ByVal ForumId As Integer, ByVal TopicID As Integer, ByVal PostID As Integer) As String
        Dim oStringaSelezione As Main.FiltroSelezioneForum
        Dim Link As String = ""
        Try
            If ForumId > 0 And TopicID > 0 And PostID > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost
            ElseIf ForumId > 0 And TopicID > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic
            ElseIf ForumId > 0 Then
                oStringaSelezione = Main.FiltroSelezioneForum.Forum
            Else
                Return ""
            End If

            'Dim RandomCode As String

            'RandomCode = COL_Persona.generaPasswordNumerica(8)
            Link = " " & Percorso

            If idPerson > 0 Then
                Link &= "&idUser=" & idPerson.ToString
            End If
            If ForumId > 0 Then
                Link &= "&ForumId=" & ForumId.ToString
            End If
            If TopicID > 0 Then
                Link &= "&TopicID=" & TopicID.ToString
            End If
            If PostID > 0 Then
                Link &= "&PostID=" & PostID.ToString
            End If
            'If oStringaSelezione = Main.FiltroSelezioneForum.Forum Then
            '    Link &= "?ForumID=" & ForumId
            '    'RandomCode = COL_Persona.generaPasswordNumerica(4)
            '    '	Link = Link & "?ExpUrl=k" & RandomCode & ForumId
            'ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic Then ' Or oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
            '    Link &= "?TopicID=" & TopicID
            '    ' RandomCode = COL_Persona.generaPasswordNumerica(5)
            '    'Link = Link & "?ExpUrl2=j" & RandomCode & TopicID
            'ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
            '    'RandomCode = COL_Persona.generaPasswordNumerica(8)
            '    Link &= "?PostID=" & PostID
            '    'Link = Link & "?for=x25jt" & RandomCode & PostID
            'End If
            'Link = Link & "&action=logon"
        Catch ex As Exception

        End Try
        Return Link
    End Function
    'Public Function LinkAccessoForumArea(ByVal PRSN_Id As Integer, ByVal Percorso As String, ByVal ForumId As Integer, ByVal TopicID As Integer, ByVal PostID As Integer) As String
    '    Dim oStringaSelezione As Main.FiltroSelezioneForum
    '    Dim Link As String = ""
    '    Try
    '        If ForumId > 0 And TopicID > 0 And PostID > 0 Then
    '            oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost
    '        ElseIf ForumId > 0 And TopicID > 0 Then
    '            oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic
    '        ElseIf ForumId > 0 Then
    '            oStringaSelezione = Main.FiltroSelezioneForum.Forum
    '        Else
    '            Return ""
    '        End If

    '        Dim RandomCode As String
    '        RandomCode = COL_Persona.generaPasswordNumerica(8)
    '        Link = " http://" & Percorso & "/AccessoForumArea.aspx" 'AddCode=" & RandomCode & CMNT_ID

    '        '  
    '        '     
    '        If oStringaSelezione = Main.FiltroSelezioneForum.Forum Then
    '            RandomCode = COL_Persona.generaPasswordNumerica(4)
    '            Link = Link & "?ExpUrl=k" & RandomCode & ForumId
    '        ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopic Then ' Or oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
    '            RandomCode = COL_Persona.generaPasswordNumerica(5)
    '            Link = Link & "?ExpUrl2=j" & RandomCode & TopicID
    '        ElseIf oStringaSelezione = Main.FiltroSelezioneForum.ForumTopicPost Then
    '            RandomCode = COL_Persona.generaPasswordNumerica(8)
    '            Link = Link & "?for=x25jt" & RandomCode & PostID
    '        End If
    '        'Link = Link & "&action=logon"
    '    Catch ex As Exception

    '    End Try


    '    Return Link

    'End Function
End Module