Imports System.Configuration

Namespace Configuration

    <Serializable(), CLSCompliant(True)>
    Public Class QuestionnaireSettings
        Public ShowDescription As Boolean
        Public ShowUrlsSettings As InvitationUrlsSettings

        Public Sub New()
            ShowDescription = False
            ShowUrlsSettings = New InvitationUrlsSettings()
        End Sub

        Public Class InvitationUrlsSettings
            Public Enabled As Boolean
            Public IsForAll As Boolean
            Public UsersTypeId As List(Of Integer)
            Public UsersId As List(Of Integer)
            Public EnabledCommunityRoles As Dictionary(Of Integer, List(Of Integer))
            Public EnabledCommunityTypeRoles As Dictionary(Of Integer, List(Of Integer))

            Public Sub New()
                Enabled = False
                IsForAll = False
                UsersTypeId = New List(Of Integer)()
                UsersId = New List(Of Integer)()
                EnabledCommunityRoles = New Dictionary(Of Integer, List(Of Integer))
                EnabledCommunityTypeRoles = New Dictionary(Of Integer, List(Of Integer))
            End Sub
        End Class


        
    End Class


End Namespace
