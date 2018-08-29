

Namespace UCServices
    Public Class Services_Forum
        Inherits Abstract.MyServices
        Public Const Codex As String = "SRVFORUM"

#Region "Public Property"
        Public Property AccessoForum() As Boolean
            Get
                AccessoForum = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Read, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Read, 0)
                End If
            End Set
        End Property
        Public Property GestioneForum() As Boolean
            Get
                GestioneForum = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 1)
                Else
                    MyBase.SetPermissionByPosition(PermissionType.Admin, 0)
                End If
            End Set
        End Property
#End Region

        Sub New()
            MyBase.New()
        End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Services_Forum
            Return New Services_Forum("00000000000000000000000000000000")
		End Function


		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			ForumList = 60003
			TopicList = 60004
			PostList = 60005
			AdminList = 60006
			UserList = 60007
			CreateTopic = 60008
			EditTopic = 60009
			CreatePost = 60010
			EditPost = 60011
			CensureTopic = 60012
            CensurePost = 60013
            CreateForum = 60014
            EditForum = 60015
            ArchiveForum = 60016
            ArchiveThread = 60017
            DeletePost = 60018
            DeleteThread = 60019
            DeleteForum = 60020
            AddedPostToAccept = 60021
            AcceptPost = 60022
            CreateCategory = 60023
            UpdateCategory = 60024
            CreateUsersGroup = 60025
            UpdateUsersGroup = 60026
			'Show = 3
			'Create = 4
			'Edit = 5
			'Clean = 6
			'VirtualDelete = 7
			'Undelete = 8
			'Delete = 9
			'Report = 10
			'Censored = 11
		End Enum
		Public Enum ObjectType
			None = 0
			Forum = 1
            Topic = 2
            Post = 3
            Category = 4
            UsersGroup = 5
        End Enum


        <Flags()> Public Enum Base2Permission
            ViewForumsList = 1 '0
            AdminService = 64 '6
        End Enum

    End Class
End Namespace