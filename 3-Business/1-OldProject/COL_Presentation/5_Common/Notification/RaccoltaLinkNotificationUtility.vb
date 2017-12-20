Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class RaccoltaLinkNotificationUtility
    Inherits BaseNotificationUtility

    Public ReadOnly Property BaseServiceUrl(ByVal CommunityID As Integer, ByVal LinkID As Integer, ByVal UrlLink As String) As String
        Get
            Return MyBase._Utility.BaseUrl & "Generici/LinkLoader.aspx?CommunityID=" & CommunityID.ToString & "&LinkID=" & LinkID.ToString & "&DestinationUrl=" & _Utility.GetUrlEncoded(UrlLink)
        End Get
    End Property

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

#Region "ADD"
    Public Sub NotifyAdd(ByVal CommunityID As Integer, ByVal LinkID As Integer, ByVal Title As String, ByVal UrlLink As String)
        Dim oValues = New List(Of String)
        oValues.Add(BaseServiceUrl(CommunityID, LinkID, UrlLink))
        '   oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(CommunityID, LinkID)))
        oValues.Add(Title)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_RaccoltaLink.ActionType.AddLink, CommunityID, Services_RaccoltaLink.Codex, oValues, CreateObjectToNotify(LinkID))
    End Sub

    Public Sub NotifyEditTitle(ByVal CommunityID As Integer, ByVal LinkID As Integer, ByVal Title As String, ByVal OldTitle As String, ByVal UrlLink As String)
        Dim oValues = New List(Of String)
        oValues.Add(BaseServiceUrl(CommunityID, LinkID, UrlLink))
        oValues.Add(Title)
        oValues.Add(OldTitle)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_RaccoltaLink.ActionType.EditLinkTitle, CommunityID, Services_RaccoltaLink.Codex, oValues, CreateObjectToNotify(LinkID))
    End Sub
    Public Sub NotifyEditUrl(ByVal CommunityID As Integer, ByVal LinkID As Integer, ByVal Title As String, ByVal UrlLink As String)
        Dim oValues = New List(Of String)
        oValues.Add(BaseServiceUrl(CommunityID, LinkID, UrlLink))
        oValues.Add(Title)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_RaccoltaLink.ActionType.EditLinkUrl, CommunityID, Services_RaccoltaLink.Codex, oValues, CreateObjectToNotify(LinkID))
    End Sub
    Public Sub NotifyEditTitleAndUrl(ByVal CommunityID As Integer, ByVal LinkID As Integer, ByVal Title As String, ByVal OldTitle As String, ByVal UrlLink As String)
        Dim oValues = New List(Of String)
        oValues.Add(BaseServiceUrl(CommunityID, LinkID, UrlLink))
        oValues.Add(Title)
        oValues.Add(OldTitle)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_RaccoltaLink.ActionType.EditLinkTitleAndUrl, CommunityID, Services_RaccoltaLink.Codex, oValues, CreateObjectToNotify(LinkID))
    End Sub
#End Region

#Region "Object To Notify"
    Private Function CreateObjectToNotify(ByVal ObjectID As Integer) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = Services_RaccoltaLink.ObjectType.Link
        obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.COL_RaccoltaLink).FullName
        obj.ModuleCode = Services_RaccoltaLink.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_RaccoltaLink.Codex)

        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_RaccoltaLink.Base2Permission.AdminService Or Services_RaccoltaLink.Base2Permission.ViewLinks
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_RaccoltaLink.Base2Permission.AdminService Or Services_RaccoltaLink.Base2Permission.Moderate
    End Function
#End Region

End Class