Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class ForumNotificationUtility
    Inherits BaseNotificationUtility


    Public ReadOnly Property BaseForumList(ByVal CommunityID As Integer) As String
        Get
            Return "Forum/LoadForum.aspx?CommunityID=" & CommunityID.ToString
        End Get
    End Property
    Public ReadOnly Property BaseForumUrl(ByVal CommunityID As Integer, ByVal ForumId As Integer) As String
        Get
            Return "Forum/LoadForum.aspx?CommunityID=" & CommunityID.ToString & "&ForumId=" & ForumId.ToString
        End Get
    End Property
    Public ReadOnly Property BaseTopicUrl(ByVal CommunityID As Integer, ByVal ForumId As Integer, ByVal TopicID As Integer) As String
        Get
            Return "Forum/LoadForum.aspx?CommunityID=" & CommunityID.ToString & "&ForumId=" & ForumId.ToString & "&TopicID=" & TopicID.ToString
        End Get
    End Property
    Public ReadOnly Property BasePostUrl(ByVal CommunityID As Integer, ByVal ForumId As Integer, ByVal TopicID As Integer, ByVal PostID As Integer) As String
        Get
            Return "Forum/LoadForum.aspx?CommunityID=" & CommunityID.ToString & "&ForumId=" & ForumId.ToString & "&TopicID=" & TopicID.ToString & "&PostID=" & PostID.ToString
        End Get
    End Property
    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

#Region "ADD"
    Public Sub NotifyAddForum(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal Title As String)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseForumList(CommunityID)))
        oValues.Add(Title)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_Forum.ActionType.CreateForum, CommunityID, Services_Forum.Codex, oValues, CreateForumObj(ForumID))
    End Sub
    Public Sub NotifyAddTopic(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal ForumTitle As String, ByVal TopicID As Integer, ByVal TopicTitle As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseForumUrl(CommunityID, ForumID)))
        oValues.Add(ForumTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseTopicUrl(CommunityID, ForumID, TopicID)))
        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Forum.ActionType.CreateTopic, CommunityID, Services_Forum.Codex, oValues, CreateTopic(ForumID, TopicID))
    End Sub
    Public Sub NotifyAddPost(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal ForumTitle As String, ByVal TopicID As Integer, ByVal TopicTitle As String, ByVal PostID As Integer, ByVal PostTitle As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseForumUrl(CommunityID, ForumID)))
        oValues.Add(ForumTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseTopicUrl(CommunityID, ForumID, TopicID)))
        oValues.Add(TopicTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BasePostUrl(CommunityID, ForumID, TopicID, PostID)))

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Forum.ActionType.CreatePost, CommunityID, Services_Forum.Codex, oValues, CreatePost(ForumID, TopicID, PostID))
    End Sub

    Public Sub NotifyPostToModerate(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal ForumTitle As String, ByVal TopicID As Integer, ByVal TopicTitle As String, ByVal PostID As Integer, ByVal PostTitle As String, ByVal OwnerID As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseForumUrl(CommunityID, ForumID)))
        oValues.Add(ForumTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseTopicUrl(CommunityID, ForumID, TopicID)))
        oValues.Add(TopicTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BasePostUrl(CommunityID, ForumID, TopicID, PostID)))
        oValues.Add(PostTitle)

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_Forum.ActionType.AddedPostToAccept, CommunityID, Services_Forum.Codex, oValues, CreatePost(ForumID, TopicID, PostID))
    End Sub
    Public Sub NotifyPostModeratedAccepted(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal ForumTitle As String, ByVal TopicID As Integer, ByVal TopicTitle As String, ByVal PostID As Integer, ByVal PostTitle As String, ByVal OwnerID As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BasePostUrl(CommunityID, ForumID, TopicID, PostID)))
        oValues.Add(ForumTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseTopicUrl(CommunityID, ForumID, TopicID)))
        oValues.Add(TopicTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BasePostUrl(CommunityID, ForumID, TopicID, PostID)))
        'oValues.Add(PostTitle)
        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Forum.ActionType.AcceptPost, CommunityID, Services_Forum.Codex, oValues, CreatePost(ForumID, TopicID, PostID))
    End Sub

    Public Sub NotifyPostCensured(ByVal CommunityID As Integer, ByVal ForumID As Integer, ByVal ForumTitle As String, ByVal TopicID As Integer, ByVal TopicTitle As String, ByVal PostID As Integer, ByVal PostTitle As String, ByVal OwnerID As Integer, ByVal OwnerName As String)

        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseForumUrl(CommunityID, ForumID)))
        oValues.Add(ForumTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseTopicUrl(CommunityID, ForumID, TopicID)))
        oValues.Add(TopicTitle)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BasePostUrl(CommunityID, ForumID, TopicID, PostID)))
        oValues.Add(PostTitle)
        oValues.Add(OwnerName)


        Dim oList As List(Of dtoNotificatedObject) = CreatePost(ForumID, TopicID, PostID)
        Dim oPersons As New List(Of Integer)
        oPersons.Add(OwnerID)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        _Utility.SendNotificationToPerson(NewsID, oPersons, Services_Forum.ActionType.CensurePost, CommunityID, Services_Forum.Codex, oValues, oList)
        Dim NewsAdminID As System.Guid = System.Guid.NewGuid
        _Utility.SendNotificationToPermission(NewsAdminID, Me.PermissionToAdmin, Services_Forum.ActionType.CensurePost, CommunityID, Services_Forum.Codex, oValues, oList)
    End Sub

#End Region

#Region "Object To Notify"
    Private Function CreateForumObj(ByVal ItemID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(ItemID, Services_Forum.ObjectType.Forum)
    End Function
    Private Function CreateTopic(ByVal ForumID As Integer, ByVal TopicID As Integer) As List(Of dtoNotificatedObject)
        Dim oList As New List(Of dtoNotificatedObject)
        oList.Add(CreateForumObj(ForumID))
        oList.Add(CreateObjectToNotify(TopicID, Services_Forum.ObjectType.Topic))
        Return oList
    End Function
    Private Function CreatePost(ByVal ForumID As Integer, ByVal TopicID As Integer, ByVal PostID As Integer) As List(Of dtoNotificatedObject)
        Dim oList As New List(Of dtoNotificatedObject)
        oList.Add(CreateForumObj(ForumID))
        oList.Add(CreateObjectToNotify(TopicID, Services_Forum.ObjectType.Topic))
        oList.Add(CreateObjectToNotify(PostID, Services_Forum.ObjectType.Post))
        Return oList
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_Forum.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Forum.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Forum.Codex)
        Select Case oType
            Case Services_Forum.ObjectType.Forum
                obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.Forum.COL_Forums).FullName
            Case Services_Forum.ObjectType.Post
                obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.Forum.COL_Forum_posts).FullName
            Case Services_Forum.ObjectType.Topic
                obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.Forum.COL_Forum_threads).FullName
        End Select
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_Forum.Base2Permission.AdminService Or Services_Forum.Base2Permission.ViewForumsList
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_Forum.Base2Permission.AdminService
    End Function
#End Region

End Class