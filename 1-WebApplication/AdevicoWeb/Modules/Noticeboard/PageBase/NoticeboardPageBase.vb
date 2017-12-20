Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
'
Public MustInherit Class NoticeboardPageBase
    Inherits PageBase
    Implements IViewNoticeboardBasePage

#Region "Implements"
    Protected Friend ReadOnly Property PreloadedIdCommunity() As Integer Implements IViewNoticeboardBasePage.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("IdCommunity")) Then
                Return CInt(Request.QueryString("IdCommunity"))
            Else
                Return PageUtility.WorkingCommunityID
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadedIdMessage() As Long Implements IViewNoticeboardBasePage.PreloadedIdMessage
        Get
            If Not IsNumeric(Request.QueryString("IdMessage")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("IdMessage"))
            End If
        End Get
    End Property
    Protected Friend Property IdCurrentMessage As Long Implements IViewNoticeboardBasePage.IdCurrentMessage
        Get
            Return ViewStateOrDefault("IdCurrentMessage", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCurrentMessage") = value
        End Set
    End Property
    Private ReadOnly Property PortalName As String Implements IViewNoticeboardBasePage.PortalName
        Get
            Return Me.Resource.getValue("PortalName")
        End Get
    End Property

#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_noticeboard", "Modules", "Noticeboard")
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleNoticeboard.ActionType) Implements IViewNoticeboardBasePage.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idMessage As Long, action As ModuleNoticeboard.ActionType) Implements IViewNoticeboardBasePage.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleNoticeboard.ObjectType.Noticeboard, idMessage.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region
#Region "Internal"
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
        End Get
    End Property
    Protected Friend Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer, Optional ByVal display As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = display

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
#End Region
End Class