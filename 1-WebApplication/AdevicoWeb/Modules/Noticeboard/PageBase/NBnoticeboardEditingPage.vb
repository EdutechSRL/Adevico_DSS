Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public MustInherit Class NBnoticeboardEditingPage
    Inherits NBnoticeboardPage
    Implements IViewNoticeboardEditing

#Region "Implements"
    Protected Friend ReadOnly Property PreloadBackUrl() As Boolean Implements IViewNoticeboardEditing.PreloadBackUrl
        Get
            Dim preload As String = Request.QueryString("lbu")
            If Not String.IsNullOrEmpty(preload) Then
                preload = preload.ToLower
            End If
            Return preload = "true"
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadFromPortal() As Boolean Implements IViewNoticeboardEditing.PreloadFromPortal
        Get
            Dim preload As String = Request.QueryString("lfp")
            If Not String.IsNullOrEmpty(preload) Then
                preload = preload.ToLower
            End If
            Return preload = "true"
        End Get
    End Property

    Protected Friend Property IdCurrentCommunity As Integer Implements IViewNoticeboardEditing.IdCurrentCommunity
        Get
            Return ViewStateOrDefault("IdCurrentCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCurrentCommunity") = value
        End Set
    End Property
    Protected Friend Property IsPortalPage As Boolean Implements IViewNoticeboardEditing.IsPortalPage
        Get
            Return ViewStateOrDefault("IsPortalPage", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPortalPage") = value
        End Set
    End Property

    Protected Friend MustOverride Sub SetHeaderTitle(isForPortal As Boolean, name As String) Implements IViewNoticeboardEditing.SetHeaderTitle
    Protected Friend MustOverride Sub SetBackUrl(url As String) Implements IViewNoticeboardEditing.SetBackUrl
    'Public Sub DisplaySessionTimeout(url As String, idCommunity As Integer) Implements IViewNoticeboardEditing.DisplaySessionTimeout
    '    RedirectOnSessionTimeOut(url, idCommunity)
    'End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, message As NoticeboardMessage, notificationUrl As String, action As ModuleNoticeboard.ActionType) Implements IViewNoticeboardEditing.SendUserAction
        If IsNothing(message) Then
            PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
        Else
            Dim oNotification As New NoticeboardNotificationUtility(Me.PageUtility)
            Dim modifiedBy As String = ""
            If IsNothing(message.ModifiedBy) Then
                modifiedBy = Resource.getValue("RemovedUserName")
            Else
                modifiedBy = message.ModifiedBy.SurnameAndName
            End If
            notificationUrl = BaseUrl & notificationUrl
            PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleNoticeboard.ObjectType.Message, message.Id), InteractionType.UserWithLearningObject)
            Select Case action
                Case ModuleNoticeboard.ActionType.Clean
                    oNotification.NotifyCleanNoticeboard(idCommunity, message.Id)
                Case ModuleNoticeboard.ActionType.Created
                    oNotification.NotifyAddMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                Case ModuleNoticeboard.ActionType.Delete
                    oNotification.NotifyDeleteMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy)
                Case ModuleNoticeboard.ActionType.Undelete
                    oNotification.NotifyUnDeleteMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                Case ModuleNoticeboard.ActionType.VirtualDelete
                    oNotification.NotifyVirtualDeleteMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                Case ModuleNoticeboard.ActionType.UndeleteAndActivate
                    oNotification.NotifyUndeleteAndActivate(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                Case ModuleNoticeboard.ActionType.SavedMessage
                    oNotification.NotifyEditMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                Case ModuleNoticeboard.ActionType.SetDefault
                    oNotification.NotifyAddMessage(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
                    '   oNotification.NotifySetDefault(message.Id, idCommunity, message.ModifiedOn, modifiedBy, notificationUrl)
            End Select
        End If
    End Sub
    Private Sub DisplayMessage(done As Boolean, action As ModuleNoticeboard.ActionType) Implements IViewNoticeboardEditing.DisplayMessage
        Dim oControl As Global.Comunita_OnLine.UC_ActionMessages = GetControlMessages()
        oControl.Visible = True
        oControl.InitializeControl(Resource.getValue("DisplayMessage.ModuleNoticeboard.ActionType." & action.ToString & "." & done.ToString), IIf(done, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.alert))
    End Sub

    Protected Friend MustOverride Function GetControlMessages() As Global.Comunita_OnLine.UC_ActionMessages
#End Region
    Protected Friend ReadOnly Property NoticeboardFilePath As String
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.Noticeboard.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Noticeboard.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.Noticeboard.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property

    Public Sub DisplaySessionTimeout1(url As String, idCommunity As Integer) Implements IViewNoticeboardEditing.DisplaySessionTimeout
        RedirectOnSessionTimeOut(url, idCommunity)
    End Sub

    Private Sub RedirectToDasboardUrl(url As String) Implements IViewNoticeboardEditing.RedirectToDasboardUrl
        PageUtility.RedirectToUrl(url)
    End Sub
End Class