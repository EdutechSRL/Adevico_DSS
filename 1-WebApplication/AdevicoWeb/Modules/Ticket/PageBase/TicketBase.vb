Imports TK = lm.Comol.Core.BaseModules.Tickets

Public MustInherit Class TicketBase
    Inherits PageBase
    Implements TK.Presentation.View.iViewBase
    
#Region "Inherits PageBase"

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Implements"

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewBase.ViewCommunityId
        Get
            Dim ComId As Integer = 0
            Try
                ComId = ViewStateOrDefault("CurrentComId", -1)
            Catch ex As Exception
            End Try

            If ComId < 0 Then
                Try
                    ComId = System.Convert.ToInt32(Request.QueryString("CommunityId"))
                Catch ex As Exception
                End Try
            End If

            Return ComId
        End Get
        Set(value As Integer)
            Me.ViewState("CurrentComId") = value
        End Set
    End Property

    Public Sub SendUserActions(ModuleId As Integer, _
                         Action As TK.ModuleTicket.ActionType, _
                         idCommunity As Integer, _
                         Type As TK.ModuleTicket.InteractionType, _
                         Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) _
                     Implements TK.Presentation.View.iViewBase.SendUserActions

        Dim oList As List(Of WS_Actions.ObjectAction) = Nothing

        If Not IsNothing(Objects) Then
            oList = (From kvp As KeyValuePair(Of Integer, String) In Objects
                    Select Me.PageUtility.CreateObjectAction(kvp.Key, kvp.Value)).ToList()
        End If

        Me.PageUtility.AddActionToModule(idCommunity, ModuleId, Action, oList, Type)

    End Sub

    Public Sub RedirectOnSessionTimeOut(ByVal DestUrl As String, ByVal CommunityId As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = DestUrl 'TK.Domain.RootObject.TicketListResolver(CommunityId)

        If CommunityId > 0 Then
            dto.IdCommunity = CommunityId
        End If

        webPost.Redirect(dto)
    End Sub

#End Region

#Region "MustOverride"

    Public MustOverride Sub DisplaySessionTimeout(CommunityId As Integer) Implements TK.Presentation.View.iViewBase.DisplaySessionTimeout

    Public MustOverride Sub ShowNoAccess() Implements TK.Presentation.View.iViewBase.ShowNoAccess

#End Region

    Public Sub SetCollapsingLit(ByVal LITmessagesExpCol As Literal, ByVal CanCollapseMessage As Boolean)

        If CanCollapseMessage Then
            LITmessagesExpCol.Visible = True

            Dim CollapseText As String = ""
            Dim ExpandText As String = ""

            Dim CollapseToolTip As String = ""
            Dim ExpandToolTip As String = ""

            Dim text As String = ""

            If Not IsNothing(Resource) Then
                CollapseText = Resource.getValue(LITmessagesExpCol.ID & ".Collapse")
                ExpandText = Resource.getValue(LITmessagesExpCol.ID & ".Expand")
                CollapseToolTip = Resource.getValue(LITmessagesExpCol.ID & ".Collapse.ToolTip")
                ExpandToolTip = Resource.getValue(LITmessagesExpCol.ID & ".Expand.ToolTip")
                text = Resource.getValue(LITmessagesExpCol.ID & ".Text")
            End If

            If String.IsNullOrEmpty(CollapseText) Then
                CollapseText = "Collapse all"
            End If

            If String.IsNullOrEmpty(ExpandText) Then
                ExpandText = "Expand all"
            End If

            If String.IsNullOrEmpty(CollapseToolTip) Then
                CollapseText = "Collapse all"
            End If

            If String.IsNullOrEmpty(ExpandToolTip) Then
                ExpandText = "Expand all"
            End If

            LITmessagesExpCol.Visible = True
            LITmessagesExpCol.Text = String.Format(LITmessagesExpCol.Text, CollapseToolTip, CollapseText, ExpandToolTip, ExpandText, text)

            'LITmessagesExpCol.Text.Replace("{ExpColmessage.Collapse}", CollapseText).Replace("{ExpColmessage.Expand}", ExpandText).Replace("{ExpColmessage.Collapse.ToolTip}", CollapseToolTip).Replace.
        Else
            LITmessagesExpCol.Visible = False
        End If
        'Dim Css As String = ""
        'If Not CanCollapseMEssage Then
        '    Css = "disabled"
        'End If

        'LITmessagesExpCol.Text = "<span class=""btngroup small " & Css & """><!--" & vbCrLf
        'LITmessagesExpCol.Text &= "--><a class=""btn first collapseall" & Css & """>"
        'LITmessagesExpCol.Text &= CollapseText
        'LITmessagesExpCol.Text &= "</a><!--" & vbCrLf
        'LITmessagesExpCol.Text &= "--><a class=""btn last expandall" & Css & """>"
        'LITmessagesExpCol.Text &= ExpandText
        'LITmessagesExpCol.Text &= "</a><!--" & vbCrLf
        'LITmessagesExpCol.Text &= "--></span>"

    End Sub


    Public Sub SendMessage(ByVal actions As IList(Of lm.Comol.Core.Notification.Domain.NotificationAction))

        'Dim action As New lm.Comol.Core.Notification.Domain.NotificationAction
        'action.IdCommunity = CInt(TXBidCommunity.Text)
        'action.IdModuleAction = CLng(TXBaction.Text)
        'action.IdObject = CLng(TXBidObject.Text)
        'action.IdObjectType = CInt(TXBobjectType.Text)
        'action.ModuleCode = DDLmodules.SelectedValue
        'Dim idUser As Integer = PageUtility.CurrentUser.ID
        'idUser = CInt(TXBidSender.Text)

        'Case 0: Found by module



    End Sub

    Public Sub SendNotification(Action As lm.Comol.Core.Notification.Domain.NotificationAction, CurrentPersonId As Integer) Implements TK.Presentation.View.iViewBase.SendNotification

        Dim service As SrvNotifications.iNotificationsManagerService = Nothing

        Try
            service = New SrvNotifications.iNotificationsManagerServiceClient()


            service.NotifyActionToModule(SystemSettings.NotificationErrorService.ComolUniqueID, Action, CurrentPersonId, PageUtility.CurrentContext.UserContext.IpAddress, PageUtility.CurrentContext.UserContext.ProxyIpAddress)


        Catch ex As Exception
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Abort()
                iService = Nothing
            End If
        Finally
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Close()
                iService = Nothing
            End If
        End Try

    End Sub

    'Public Sub SendNotifications(Actions As IList(Of lm.Comol.Core.Notification.Domain.NotificationAction), CurrentPersonId As Integer) Implements TK.Presentation.View.iViewBase.SendNotifications

    '    Dim service As SrvNotifications.iNotificationsManagerService = Nothing

    '    Try
    '        service = New SrvNotifications.iNotificationsManagerServiceClient()

    '        For Each action As lm.Comol.Core.Notification.Domain.NotificationAction In Actions
    '            service.NotifyActionToModule(SystemSettings.NotificationErrorService.ComolUniqueID, action, CurrentPersonId, PageUtility.CurrentContext.UserContext.IpAddress, PageUtility.CurrentContext.UserContext.ProxyIpAddress)
    '        Next

    '    Catch ex As Exception
    '        If Not IsNothing(service) Then
    '            Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
    '            iService.Abort()
    '            iService = Nothing
    '        End If
    '    Finally
    '        If Not IsNothing(service) Then
    '            Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
    '            iService.Close()
    '            iService = Nothing
    '        End If
    '    End Try

    'End Sub

    Private _CssVerison As String = ""

    Public Function CssVersion() As String

        If String.IsNullOrEmpty(_CssVerison) Then
            Dim tkVerUC As UC_TicketCssVersion = LoadControl(BaseUrl & "/Modules/Ticket/UC/UC_TicketCssVersion.ascx")
            _CssVerison = TkVerUC.GetVersionString()
        End If

        Return _CssVerison
        'Return "?v=201507080935lm"
    End Function
End Class