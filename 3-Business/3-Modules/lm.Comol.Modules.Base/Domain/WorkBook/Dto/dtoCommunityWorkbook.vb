Imports lm.Comol.Core.DomainModel
Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoCommunityWorkbook
        Public WorkBookID As System.Guid
        Public CommunityID As Integer
        Public CommunityName As String
        Public WorkBookObject As WorkBook
        Sub New()

        End Sub
        Sub New(ByVal pId As System.Guid, ByVal pCommunity As iCommunity)
            Me.WorkBookID = pId
            If IsNothing(pCommunity) Then
                Me.CommunityID = 0
                Me.CommunityName = ""
            Else
                Me.CommunityID = pCommunity.Id
                Me.CommunityName = pCommunity.Name
            End If
        End Sub
        Sub New(ByVal pId As System.Guid, ByVal pCommunity As iCommunity, ByVal PortalName As String)
            Me.WorkBookID = pId
            If IsNothing(pCommunity) Then
                Me.CommunityID = 0
                Me.CommunityName = PortalName
            Else
                Me.CommunityID = pCommunity.Id
                Me.CommunityName = pCommunity.Name
            End If
        End Sub
        Sub New(ByVal pId As System.Guid, ByVal pWorkBook As WorkBook)
            Me.WorkBookID = pId
            Me.WorkBookObject = pWorkBook
            If IsNothing(pWorkBook.CommunityOwner) Then
                Me.CommunityID = 0
                Me.CommunityName = ""
            Else
                Me.CommunityID = pWorkBook.CommunityOwner.Id
                Me.CommunityName = pWorkBook.CommunityOwner.Name
            End If
        End Sub
        Sub New(ByVal pId As System.Guid, ByVal pWorkBook As WorkBook, ByVal PortalName As String)
            Me.WorkBookID = pId
            Me.WorkBookObject = pWorkBook
            If IsNothing(pWorkBook.CommunityOwner) Then
                Me.CommunityID = 0
                Me.CommunityName = PortalName
            Else
                Me.CommunityID = pWorkBook.CommunityOwner.Id
                Me.CommunityName = pWorkBook.CommunityOwner.Name
            End If
        End Sub
        Sub New(ByVal pWorkBook As WorkBook, ByVal PortalName As String)
            Me.WorkBookID = pWorkBook.Id
            Me.WorkBookObject = pWorkBook
            If IsNothing(pWorkBook.CommunityOwner) Then
                Me.CommunityID = 0
                Me.CommunityName = PortalName
            Else
                Me.CommunityID = pWorkBook.CommunityOwner.Id
                Me.CommunityName = pWorkBook.CommunityOwner.Name
            End If
        End Sub
        Sub New(ByVal pWorkBook As WorkBook, ByVal AllCommunityID As Integer, ByVal AllCommunityName As String)
            Me.WorkBookID = pWorkBook.Id
            Me.WorkBookObject = pWorkBook
            Me.CommunityID = AllCommunityID
            Me.CommunityName = AllCommunityName
        End Sub
    End Class
End Namespace