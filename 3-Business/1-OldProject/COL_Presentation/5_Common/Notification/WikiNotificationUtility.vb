Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class WikiNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public ReadOnly Property BaseServiceUrl() As String
        Get
            Return "Wiki/Wiki_Comunita.aspx"
        End Get
    End Property
    Public ReadOnly Property BaseServiceTopicUrl(ByVal TopicID As System.Guid) As String
        Get
            Return "Wiki/Wiki_Comunita.aspx?ID=" & TopicID.ToString
        End Get
    End Property
    Public ReadOnly Property BaseServiceSectionUrl(ByVal SectionID As System.Guid) As String
        Get
            Return "Wiki/Wiki_Comunita.aspx?SectionID=" & SectionID.ToString
        End Get
    End Property
    Public Sub NotifyTopicAdd(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal CreatorName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceTopicUrl(TopicID)))
        oValues.Add(Title)

        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.CreateTopic, CommunityID, Services_Wiki.Codex, oValues, CreateTopicToNotify(TopicID))
    End Sub
    Public Sub NotifyTopicEdit(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal UserName As String)
        Dim oValues = New List(Of String)
        oValues.Add(UserName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceTopicUrl(TopicID)))
        oValues.Add(Title)

        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.EditTopic, CommunityID, Services_Wiki.Codex, oValues, CreateTopicToNotify(TopicID))
    End Sub
    Public Sub NotifyTopicRipristina(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal DataCreation As DateTime, ByVal UserName As String)
        Dim oValues = New List(Of String)
        oValues.Add(UserName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceTopicUrl(TopicID)))
        oValues.Add(Title)
        oValues.Add(FormatDateTime(DataCreation, DateFormat.ShortDate) & " " & FormatDateTime(DataCreation, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.ResumeTopic, CommunityID, Services_Wiki.Codex, oValues, CreateTopicToNotify(TopicID))
    End Sub
    Public Sub NotifyTopicDelete(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal UserName As String)
        Dim oValues = New List(Of String)
        oValues.Add(UserName)
        oValues.Add(Title)
        'oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(TopicID)))
        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.DeleteTopic, CommunityID, Services_Wiki.Codex, oValues, CreateTopicToNotify(TopicID))
    End Sub
    Public Sub NotifySectionAdd(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionName As String, ByVal SectionId As System.Guid)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(SectionName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceSectionUrl(SectionId)))

        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.CreateSection, CommunityID, Services_Wiki.Codex, oValues, CreateSectionToNotify(SectionId))
    End Sub
    Public Sub NotifySectionEdit(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal PreviousName As String, ByVal NewName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(PreviousName)
        oValues.Add(NewName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceSectionUrl(SectionId)))
        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.EditSection, CommunityID, Services_Wiki.Codex, oValues, CreateSectionToNotify(SectionId))
    End Sub
    Public Sub NotifySectionDelete(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal SectionName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(SectionName)
        '  oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))
        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.DeleteSection, CommunityID, Services_Wiki.Codex, oValues, CreateSectionToNotify(SectionId))
    End Sub
    Public Sub NotifySectionRipristina(ByVal CommunityID As Integer, ByVal SectionId As System.Guid, ByVal SectionName As String, ByVal DataCreation As DateTime, ByVal UserName As String)
        Dim oValues = New List(Of String)
        oValues.Add(UserName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceSectionUrl(SectionId)))
        oValues.Add(SectionName)
        oValues.Add(FormatDateTime(DataCreation, DateFormat.ShortDate) & " " & FormatDateTime(DataCreation, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSeeTopic, Services_Wiki.ActionType.ResumeTopic, CommunityID, Services_Wiki.Codex, oValues, CreateTopicToNotify(SectionId))
    End Sub
#Region "Object To Notify"
    Private Function CreateTopicToNotify(ByVal TopicID As System.Guid) As dtoNotificatedObject
        Return CreateObjectToNotify(TopicID, Services_Wiki.ObjectType.Topic)
    End Function
    Private Function CreateSectionToNotify(ByVal SectionID As System.Guid) As dtoNotificatedObject
        Return CreateObjectToNotify(SectionID, Services_Wiki.ObjectType.Section)
    End Function
    Private Function CreateWikiToNotify(ByVal WikiID As System.Guid) As dtoNotificatedObject
        Return CreateObjectToNotify(WikiID, Services_Wiki.ObjectType.Wiki)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As System.Guid, ByVal oType As Services_WorkBook.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Wiki.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Wiki.Codex)
        If oType = Services_Wiki.ObjectType.Wiki Then
            obj.FullyQualiFiedName = GetType(COL_Wiki.WikiNew.Wiki).FullName
        ElseIf oType = Services_Wiki.ObjectType.Section Then
            obj.FullyQualiFiedName = GetType(COL_Wiki.WikiNew.SezioneWiki).FullName
        ElseIf oType = Services_Wiki.ObjectType.Topic Then
            obj.FullyQualiFiedName = GetType(COL_Wiki.WikiNew.TopicWiki).FullName
        End If
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSeeTopic() As Integer
        Return Services_Wiki.Base2Permission.AdminService Or Services_Wiki.Base2Permission.ManageWiki Or Services_Wiki.Base2Permission.ViewTopic Or Services_Wiki.Base2Permission.ManageSection Or Services_Wiki.Base2Permission.ManageTopics  ' Or Services_WorkBook.Base2Permission.
    End Function
    Public Function PermissionToSeeSection() As Integer
        Return Services_Wiki.Base2Permission.AdminService Or Services_Wiki.Base2Permission.ManageWiki Or Services_Wiki.Base2Permission.ManageSection    ' Or Services_WorkBook.Base2Permission.
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_Wiki.Base2Permission.AdminService Or Services_Wiki.Base2Permission.ManageWiki
    End Function
#End Region

End Class