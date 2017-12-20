Public Class PresenterSearchUserByCommunities
    Inherits GenericPresenter

    Private _SelectedCommunitiesId As List(Of Integer)
    Private _Manager As ManagerSubscription

    Private ReadOnly Property CurrentManager() As ManagerSubscription
        Get
            If _Manager Is Nothing Then
                _Manager = New ManagerSubscription(Nothing, Nothing, Me.View.CurrentLanguage)
            End If
            Return _Manager
        End Get
    End Property
    Private Shadows ReadOnly Property View() As IviewSearchUser
        Get
            View = MyBase.view
        End Get
    End Property

    Public Sub New(ByVal view As IviewSearchUser)
        MyBase.view = view
    End Sub

    Private Function LoadRolesByCommunities() As List(Of Role)
        Dim oRoleList As List(Of Role) = Me.CurrentManager.GetRolesByCommunity(Me.View.SelectedCommunitiesId)

        Dim QueryOrdinamento = From oRole As Role In oRoleList Order By oRole.Name Ascending

        If QueryOrdinamento.Count > 0 Then
            oRoleList = QueryOrdinamento.ToList

        End If
        'se non ci sono ruoli si aggiunge quello di defalut, se ce ne sono si aggiunge solo se non c'e' gia'
        If oRoleList.Count = 0 OrElse Not oRoleList.Item(0).ID = -1 Then
            Dim oRoleDefault As New Role(-1, "- -", 9999)
            oRoleDefault.Name = "-----"
            oRoleList.Insert(0, oRoleDefault)
        End If
        Return oRoleList
    End Function
    Public Sub setSingleUser(ByVal userId As Integer)
        Dim oUser As New MemberContact
        Dim oUserList As New List(Of MemberContact)
        Dim oTempUserList As New List(Of MemberContact)
        oUserList = Me.CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.Name, Me.View.Surname, Me.View.RegistrationCode, Me.View.MailAddress, Me.View.Login, Me.View.SelectedRoleId, Me.View.CurrentUserId)
        Dim oMemberContact = From oMember As MemberContact In oUserList Where oMember.Id = userId
        oUser = oMemberContact.Single
        oTempUserList.Add(oUser)
        CurrentManager.TemporaryUserList(Me.View.CurrentUserId) = oTempUserList
    End Sub
    Private Sub TemporaryUserListUpdate(Optional ByVal IdToRemove As Integer = 0)
        Dim oUserList As New List(Of MemberContact)
        Dim oOldTemporaryUserList As New List(Of MemberContact)
        oOldTemporaryUserList = CurrentManager.TemporaryUserList(Me.View.CurrentUserId)
        'oUserList = Me.CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.Name, Me.View.Surname, Me.View.RegistrationCode, Me.View.MailAddress, Me.View.Login, Me.View.SelectedRoleId, Me.View.CurrentUserId)
        oUserList = Me.CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.CurrentUserId)
        Dim oNewUserIdList As New List(Of BaseElement)
        oNewUserIdList = Me.View.GetUsers()
        If oNewUserIdList.Count > 0 Then
            For Each oElement As BaseElement In oNewUserIdList

                Dim id As Integer = oElement.id
                Dim oMemberContact = From oMember As MemberContact In oUserList Where oMember.Id = id
                If oMemberContact.Count > 0 Then
                    If oElement.isSelected Then
                        'se e' gia' nella lista lo toglie, se non c'e' lo aggiunge
                        If Not oOldTemporaryUserList.Contains(oMemberContact.Single) Then
                            oOldTemporaryUserList.Add(oMemberContact.Single)
                        End If
                    Else
                        If oOldTemporaryUserList.Contains(oMemberContact.Single) Then
                            oOldTemporaryUserList.Remove(oMemberContact.Single)
                        End If
                    End If
                End If
            Next
        End If
        If Not IdToRemove = 0 Then
            Dim oMemberContact = From oMember As MemberContact In oOldTemporaryUserList Where oMember.Id = IdToRemove
            If oOldTemporaryUserList.Count > 0 AndAlso Not IsNothing(oMemberContact) AndAlso oOldTemporaryUserList.Contains(oMemberContact.Single) Then
                oOldTemporaryUserList.Remove(oMemberContact.Single)
            End If
        End If
        CurrentManager.TemporaryUserList(Me.View.CurrentUserId) = oOldTemporaryUserList
    End Sub
    Public Sub RemoveUserFromPreview(ByVal id As Integer)
        TemporaryUserListUpdate(id)
        Dim oOldUserList As New List(Of MemberContact)
        Dim oSearchResult As New List(Of MemberContact)
        oOldUserList = CurrentManager.TemporaryUserList(Me.View.CurrentUserId)
        Me.View.BindPreview(oOldUserList)
        oSearchResult = Me.CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.Name, Me.View.Surname, Me.View.RegistrationCode, Me.View.MailAddress, Me.View.Login, Me.View.SelectedRoleId, Me.View.CurrentUserId)
        BindSearchResult(oSearchResult)
    End Sub
    Public Sub BindPreview()
        If Me.View.SelectionMode = ListSelectionMode.Multiple Then
            TemporaryUserListUpdate()
        End If
        If Me.View.PreviewUserList_isVisible Then
            Me.View.BindPreview(CurrentManager.TemporaryUserList(Me.View.CurrentUserId))
        End If
    End Sub
    Public Function GetTemporaryUserIdList() As List(Of Integer)
        Dim oList As New List(Of MemberContact)
        Dim oIdList As New List(Of Integer)
        oList = CurrentManager.TemporaryUserList(Me.View.CurrentUserId)
        For Each oMember As MemberContact In oList
            oIdList.Add(oMember.Id)
        Next
        Return oIdList
    End Function
    Private Sub BindSearchResult(ByRef oUserList As List(Of MemberContact))
        Dim ElementsToHide As List(Of Integer)
        ElementsToHide = CurrentManager.ElementsToHide(Me.View.CurrentUserId)
        If Not ElementsToHide Is Nothing Then
            For Each id As Integer In ElementsToHide
                Dim idToRemove As Integer = id
                Dim oUserToRemove = From element As MemberContact In oUserList Where element.Id = idToRemove
                If oUserToRemove.Count > 0 Then
                    oUserList.Remove(oUserToRemove.Single)
                End If
            Next
        End If
        Me.View.BindSearchResult(oUserList)
    End Sub
    Public Sub Search(Optional ByVal ULupdate As Boolean = True)
        If ULupdate Then
            TemporaryUserListUpdate()
        End If
        BindSearchResult(Me.CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.Name, Me.View.Surname, Me.View.RegistrationCode, Me.View.MailAddress, Me.View.Login, Me.View.SelectedRoleId, Me.View.CurrentUserId))
    End Sub
    Public Function GetConfirmedUsers() As List(Of MemberContact)
        Dim oList As List(Of MemberContact)
        If Me.View.SelectionMode = ListSelectionMode.Multiple Then
            TemporaryUserListUpdate()
        End If
        oList = CurrentManager.TemporaryUserList(Me.View.CurrentUserId)
        Return oList
    End Function
    Public Sub GoToPage(ByVal PageIndex As Integer)
        Dim oLista As IList = CurrentManager.GetUserList(Me.View.SelectedCommunitiesId, Me.View.Name, Me.View.Surname, Me.View.RegistrationCode, Me.View.MailAddress, Me.View.Login, Me.View.SelectedRoleId, Me.View.CurrentUserId)
        If oLista.Count = 0 Then
            Me.View.GridCurrentPage = 1
        Else
            Me.View.GridMaxPage = Math.Ceiling(oLista.Count / Me.View.GridPageSize)
            If PageIndex > Me.View.GridMaxPage Then
                PageIndex = Me.View.GridMaxPage
            End If
            Me.View.GridCurrentPage = PageIndex
        End If
        BindSearchResult(oLista)
    End Sub
    Public Sub Init(ByRef oCommunitiesIdList As List(Of Integer), ByRef SelectionMode As ListSelectionMode, Optional ByRef oElementToHideIdList As List(Of Integer) = Nothing, Optional ByRef ShowMail As Boolean = False)
        Dim oList As New List(Of MemberContact)
        'Me.CurrentManager.ResetSelectedUserListCache(oCommunitiesIdList)
        Me.CurrentManager.TemporaryUserList(Me.View.CurrentUserId) = oList
        Me.View.SelectedCommunitiesId = oCommunitiesIdList
        Me.View.showMail = ShowMail
        Me.View.init(LoadRolesByCommunities())
        Me.View.SelectionMode = SelectionMode
        Me.CurrentManager.ElementsToHide(Me.View.CurrentUserId) = oElementToHideIdList
    End Sub

End Class
