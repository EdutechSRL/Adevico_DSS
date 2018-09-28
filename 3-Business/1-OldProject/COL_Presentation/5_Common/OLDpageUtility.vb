Imports COL_BusinessLogic_v2.Comol.Entities
Imports COL_BusinessLogic_v2.Comol.Manager
Imports COL_BusinessLogic_v2.Comunita
Imports System.Web
Imports lm.ActionDataContract
Imports lm.Notification.DataContract.Domain
Imports System.Linq
Imports FederationNS = lm.Comol.Core.BaseModules.Federation

Public Class OLDpageUtility

#Region "Private"
    Private _Context As HttpContext
    Private _Application As HttpApplicationState
    Private _Request As HttpRequest
    Private _Server As HttpServerUtility
    Private _Response As HttpResponse
    Private _Session As System.Web.SessionState.HttpSessionState
    Private _FilePath As ObjectFilePath
    Private _Configurations As Hashtable
    Private _BaseUserRepositoryPath As ObjectFilePath
    Private _ActionService As iRemoteService
    Private _ServiziCorrenti As ServiziCorrenti
    Private _RegisterAction As WS_Actions.WSuserActionsSoapClient
    Private _TrapSender As WsSnmtp.WsSnmtpSoapClient
    Private _NotificationManager As lm.Modules.NotificationSystem.Business.ManagerCommunitynews
    Private _CommunitiesNews As Dictionary(Of Integer, List(Of lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount))
    Private _PermissionService As PermissionService.IServicePermission
    Private _BaseUrl As String
    Private _SecureBaseUrl As String
    Private _AllowRegisterActions As Boolean?
#End Region

#Region "Services"
    Private _manager As lm.Comol.Core.Business.BaseModuleManager

#Region "Definition"
    Private ReadOnly Property BaseModuleManager() As lm.Comol.Core.Business.BaseModuleManager
        Get
            If IsNothing(_manager) Then
                _manager = New lm.Comol.Core.Business.BaseModuleManager(CurrentContext)
            End If
            Return _manager
        End Get
    End Property
    Public ReadOnly Property PermissionService() As PermissionService.IServicePermission
        Get
            Dim oSender As PermissionService.ServicePermissionClient = Nothing
            Try
                oSender = New PermissionService.ServicePermissionClient
            Catch ex As Exception

            End Try
            Return oSender

            'If IsNothing(_PermissionService) Then
            '    Dim oSender As PermissionService.IServicePermission = Nothing
            '    Try
            '        _PermissionService = New PermissionService.ServicePermissionClient
            '    Catch ex As Exception

            '    End Try
            'End If
            'Return _PermissionService
        End Get
    End Property

    Protected ReadOnly Property RegisterAction() As WS_Actions.WSuserActionsSoapClient
        Get
            If IsNothing(_RegisterAction) Then
                Try
                    _RegisterAction = New WS_Actions.WSuserActionsSoapClient
                Catch ex As Exception

                End Try
            End If
            Return _RegisterAction
        End Get
    End Property

    Protected ReadOnly Property TrapSender() As WsSnmtp.WsSnmtpSoapClient
        Get
            If IsNothing(_TrapSender) Then
                Try
                    _TrapSender = New WsSnmtp.WsSnmtpSoapClient
                Catch ex As Exception

                End Try
            End If
            Return _TrapSender
        End Get
    End Property

    Protected ReadOnly Property NotificationSender() As NotificationService.iNotificationServiceClient
        Get
            Dim oSender As NotificationService.iNotificationServiceClient = Nothing
            Try
                oSender = New NotificationService.iNotificationServiceClient
            Catch ex As Exception

            End Try
            Return oSender
        End Get
    End Property
    Protected ReadOnly Property NotificationManagement() As lm.Modules.NotificationSystem.WSremoteManagement.NotificationManagementSoapClient
        Get
            Dim oSender As lm.Modules.NotificationSystem.WSremoteManagement.NotificationManagementSoapClient = Nothing
            Try
                oSender = New lm.Modules.NotificationSystem.WSremoteManagement.NotificationManagementSoapClient
            Catch ex As Exception

            End Try
            Return oSender
        End Get
    End Property


#End Region

#Region "Service Notification"
#Region "Notification Community News"

    Public Function CommunityNewsCount(ByVal PersonID As Integer, ByVal CommunityID As Integer) As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount
        Dim oCurrent As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount = (From n In CommunitiesNews(PersonID) Where n.ID = CommunityID).FirstOrDefault
        If IsNothing(oCurrent) Then
            oCurrent = New lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount With {.Count = 0, .ID = CommunityID}
        End If
        Return oCurrent

    End Function

    Public Function GetCommunityNewsUrl(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal CurrentView As lm.Modules.NotificationSystem.Domain.ViewModeType) As String
        Dim FromDay As DateTime = DateTime.MinValue
        Dim oSubscription As lm.Comol.Core.DomainModel.Subscription = Me.NotificationManager.GetSubscription(CommunityID, PersonID)
        Dim Url As String = ""
        If Not IsNothing(oSubscription) Then
            If oSubscription.LastAccessOn.HasValue Then
                FromDay = oSubscription.LastAccessOn.Value
            Else
                FromDay = Now.Date.AddDays(-30)
            End If
            Url = Me.BaseUrl & "Notification/CommunityNews.aspx?FromDay=" & Me.GetUrlEncoded(FromDay.ToString) & "&PageSize=25&Page=0&CommunityID=" & CommunityID.ToString & "&PR_View=" & CurrentView.ToString
        End If
        Return Url
    End Function

    Public Sub SendNotificationUpdateCommunityAccess(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LastAccessTime As DateTime)
        Dim Sender As lm.Modules.NotificationSystem.WSremoteManagement.NotificationManagementSoapClient = Me.NotificationManagement
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oCurrent As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount = (From n In CommunitiesNews(PersonID) Where n.ID = CommunityID).FirstOrDefault
                Dim oRemote As New lm.Modules.NotificationSystem.WSremoteManagement.dtoCommunityWithNews

                oRemote.CommunityID = CommunityID
                oRemote.PersonID = PersonID
                If oCurrent Is Nothing Then
                    oRemote.ActionCount = 0
                    oRemote.LastUpdate = Nothing
                Else
                    oRemote.ActionCount = oCurrent.Count
                    oRemote.LastUpdate = oCurrent.LastUpdate
                    oCurrent.Count = 0
                    oCurrent.LastUpdate = LastAccessTime
                End If
                Sender.UpdateCommunityNewsCount(oRemote)
                '    Me.NotificationManager.ClearCommunityNewsCacheItems(PersonID)
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

#Region "Notification Sender"
    Public Sub SendNotificationToCommunity(ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        SendNotificationToCommunity(ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub
    Public Sub SendNotificationToPerson(ByVal PersonsID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        Dim NewsID As System.Guid = Guid.NewGuid
        SendNotificationToPerson(NewsID, PersonsID, ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub
    Public Sub SendNotificationToPerson(ByVal NewsID As System.Guid, ByVal PersonsID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        SendNotificationToPerson(NewsID, PersonsID, ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub
    Public Sub SendNotificationToPermission(ByVal Permissions As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String))
        Dim NewsID As System.Guid = System.Guid.NewGuid
        SendNotificationToPermission(NewsID, Permissions, ActionID, CommunityID, ModuleCode, Parameters, New List(Of dtoNotificatedObject))
    End Sub
    Public Sub SendNotificationToPermission(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String))
        SendNotificationToPermission(NewsID, Permissions, ActionID, CommunityID, ModuleCode, Parameters, New List(Of dtoNotificatedObject))
    End Sub
    Public Sub SendNotificationToPermission(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        SendNotificationToPermission(NewsID, Permissions, ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub
    Public Sub SendNotificationToPermission(ByVal Permissions As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        SendNotificationToPermission(NewsID, Permissions, ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub
    Public Sub SendNotificationToRoles(ByVal NewsID As System.Guid, ByVal RolesID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String))
        SendNotificationToRoles(NewsID, RolesID, ActionID, CommunityID, ModuleCode, Parameters, New List(Of dtoNotificatedObject))
    End Sub
    Public Sub SendNotificationToRoles(ByVal NewsID As System.Guid, ByVal RolesID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal obj As dtoNotificatedObject)
        Dim Objects As New List(Of dtoNotificatedObject)
        If Not IsNothing(obj) Then
            Objects.Add(obj)
        End If
        SendNotificationToRoles(NewsID, RolesID, ActionID, CommunityID, ModuleCode, Parameters, Objects)
    End Sub

    Public Sub SendNotificationToCommunity(ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.Objects = Objects
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                Sender.NotifyToCommunity(CreateNotificationToCommunity(oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub
    Public Sub SendNotificationToPerson(ByVal NewsID As System.Guid, ByVal PersonsID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.Objects = Objects
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                Sender.NotifyToUsers(CreateNotificationToPerson(NewsID, PersonsID, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub
    Public Sub SendNotificationToPermission( _
                                           ByVal NewsID As System.Guid, _
                                           ByVal Permissions As Integer, _
                                           ByVal ActionID As Integer, _
                                           ByVal CommunityID As Integer, _
                                           ByVal ModuleCode As String, _
                                           ByVal Parameters As List(Of String), _
                                           ByVal Objects As List(Of dtoNotificatedObject))

        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender

        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled AndAlso Me.SystemSettings.NotificationService.isServiceEnabled(ModuleCode) Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.Objects = Objects
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                Sender.NotifyForPermission(CreateNotificationToPermission(NewsID, Permissions, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub

    Public Sub SendNotificationToRoles(ByVal NewsID As System.Guid, ByVal RolesID As List(Of Integer), ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.Objects = Objects
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                Sender.NotifyForRoles(CreateNotificationToRole(NewsID, RolesID, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub

    Public Sub SendNotificationToItemInt(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As Integer, ByVal ItemTypeId As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                oContext.Objects = Objects
                Sender.NotifyForPermissionItemInt(CreateNotificationToItemInt(NewsID, Permissions, ItemID, ItemTypeId, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub
    Public Sub SendNotificationToItemLong(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As Long, ByVal ItemTypeId As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal oDto As dtoNotificatedObject)
        Dim oList As New List(Of dtoNotificatedObject)
        If Not IsNothing(oDto) Then
            oList.Add(oDto)
        End If
        Me.SendNotificationToItemLong(NewsID, Permissions, ItemID, ItemTypeId, ActionID, CommunityID, ModuleCode, Parameters, oList)
    End Sub
    Public Sub SendNotificationToItemLong(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As Long, ByVal ItemTypeId As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                oContext.Objects = Objects
                Sender.NotifyForPermissionItemLong(CreateNotificationToItemLong(NewsID, Permissions, ItemID, ItemTypeId, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub
    Public Sub SendNotificationToItemGuid(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As System.Guid, ByVal ItemTypeId As Integer, ByVal ActionID As Integer, ByVal CommunityID As Integer, ByVal ModuleCode As String, ByVal Parameters As List(Of String), ByVal Objects As List(Of dtoNotificatedObject))
        Dim Sender As NotificationService.iNotificationServiceClient = Me.NotificationSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf Me.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oContext As New dtoNotificationContex
                oContext.Parameters = Parameters
                oContext.ModuleID = Me.GetModuleID(ModuleCode)
                oContext.ModuleCode = ModuleCode
                oContext.CommunityID = CommunityID
                oContext.ActionID = ActionID
                oContext.Objects = Objects
                Sender.NotifyForPermissionItemGuid(CreateNotificationToItemGuid(NewsID, Permissions, ItemID, ItemTypeId, oContext))
            Catch ex As Exception

            End Try
            DisposeNotificationSender(Sender)
        End If
    End Sub


    Private Function CreateNotificationToCommunity(ByVal Context As dtoNotificationContex) As lm.Notification.DataContract.Domain.NotificationToCommunity
        Dim oNotification As New NotificationToCommunity(System.Guid.NewGuid)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            '.ValueParameters = New WS_NotificationSender.ArrayOfString
            'If Not (Context.Parameters Is Nothing OrElse Context.Parameters.Count = 0) Then
            .ValueParameters.AddRange(Context.Parameters)
            'End If

        End With
        Return oNotification
    End Function
    Private Function CreateNotificationToPermission( _
                                                   ByVal NewsID As System.Guid, _
                                                   ByVal Permissions As Integer, _
                                                   ByVal Context As dtoNotificationContex) _
                                               As lm.Notification.DataContract.Domain.NotificationToPermission

        Dim oNotification As New NotificationToPermission(NewsID)

        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            '.ValueParameters = New WS_NotificationSender.ArrayOfString
            'If Not (Context.Parameters Is Nothing OrElse Context.Parameters.Count = 0) Then
            .ValueParameters.AddRange(Context.Parameters)
            'End If
            .Permission = Permissions
        End With

        Return oNotification
    End Function
    'Private Function CreateNotificationToPerson(ByVal PersonsID As List(Of Integer), ByVal Context As dtoNotificationContex) As NotificationToPerson
    '    Dim oNotification As New NotificationToPerson(System.Guid.NewGuid)
    '    With oNotification
    '        .ActionID = Context.ActionID
    '        .CommunityID = Context.CommunityID
    '        .ModuleCode = Context.ModuleCode
    '        .ModuleID = Context.ModuleID
    '        .NotificatedObjects = Context.Objects
    '        .SentDate = Now
    '        '.ValueParameters = New WS_NotificationSender.ArrayOfString
    '        'If Not (Context.Parameters Is Nothing OrElse Context.Parameters.Count = 0) Then
    '        .ValueParameters.AddRange(Context.Parameters)
    '        'End If
    '        '.PersonsID = New WS_NotificationSender.ArrayOfInt
    '        'If PersonsID.Count > 0 Then
    '        .PersonsID.AddRange(PersonsID)
    '        'End If
    '    End With

    '    Return oNotification
    'End Function
    Private Function CreateNotificationToPerson(ByVal NewsID As System.Guid, ByVal PersonsID As List(Of Integer), ByVal Context As dtoNotificationContex) As NotificationToPerson
        Dim oNotification As New NotificationToPerson(NewsID)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            .ValueParameters.AddRange(Context.Parameters)
            .PersonsID.AddRange(PersonsID)
        End With

        Return oNotification
    End Function
    Private Function CreateNotificationToRole(ByVal NewsID As System.Guid, ByVal RolesID As List(Of Integer), ByVal Context As dtoNotificationContex) As NotificationToRole
        Dim oNotification As New NotificationToRole(NewsID)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            .ValueParameters.AddRange(Context.Parameters)
            .RolesID.AddRange(RolesID)
        End With
        Return oNotification
    End Function
    Private Function CreateNotificationToItemInt(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As Integer, ByVal ItemTypeID As Integer, ByVal Context As dtoNotificationContex) As NotificationToItemInt
        Dim oNotification As New NotificationToItemInt(NewsID)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            .ValueParameters.AddRange(Context.Parameters)
            .ItemID = ItemID
            .ObjectTypeID = ItemTypeID
            .Permission = Permissions
        End With
        Return oNotification
    End Function
    Private Function CreateNotificationToItemLong(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As Long, ByVal ItemTypeID As Integer, ByVal Context As dtoNotificationContex) As NotificationToItemLong
        Dim oNotification As New NotificationToItemLong(NewsID)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            .ValueParameters.AddRange(Context.Parameters)
            .ItemID = ItemID
            .ObjectTypeID = ItemTypeID
            .Permission = Permissions
        End With
        Return oNotification
    End Function
    Private Function CreateNotificationToItemGuid(ByVal NewsID As System.Guid, ByVal Permissions As Integer, ByVal ItemID As System.Guid, ByVal ItemTypeID As Integer, ByVal Context As dtoNotificationContex) As NotificationToItemGuid
        Dim oNotification As New NotificationToItemGuid(NewsID)
        With oNotification
            .ActionID = Context.ActionID
            .CommunityID = Context.CommunityID
            .ModuleCode = Context.ModuleCode
            .ModuleID = Context.ModuleID
            .NotificatedObjects = Context.Objects
            .SentDate = Now
            .ValueParameters.AddRange(Context.Parameters)
            .ItemID = ItemID
            .ObjectTypeID = ItemTypeID
            .Permission = Permissions
        End With
        Return oNotification
    End Function

    Private Class dtoNotificationContex
        Public ActionID As Integer
        Public CommunityID As Integer
        Public ModuleCode As String
        Public ModuleID As Integer
        Public Parameters As New List(Of String)
        Public Objects As New List(Of dtoNotificatedObject)
    End Class

    Private Function UpdateNotificatedObjects(ByVal objects As List(Of dtoNotificatedObject), ByVal ModuleID As String, ByVal ModuleCode As String) As List(Of dtoNotificatedObject)
        Dim oList As New List(Of dtoNotificatedObject)

        oList.AddRange((From o In objects Select New dtoNotificatedObject() With {.FullyQualiFiedName = o.FullyQualiFiedName, .ModuleCode = ModuleCode, .ModuleID = ModuleID, .ObjectID = o.ObjectID, .ObjectTypeID = o.ObjectTypeID}).ToList)

        Return oList
    End Function

    Private Sub DisposeNotificationSender(ByVal oSender As NotificationService.iNotificationService)
        If Not IsNothing(oSender) Then
            Dim service As System.ServiceModel.ClientBase(Of NotificationService.iNotificationService) = DirectCast(oSender, System.ServiceModel.ClientBase(Of NotificationService.iNotificationService))
            Try
                If service.State <> System.ServiceModel.CommunicationState.Closed AndAlso service.State <> System.ServiceModel.CommunicationState.Faulted Then
                    service.Close()
                ElseIf service.State <> System.ServiceModel.CommunicationState.Closed Then
                    service.Abort()
                End If
                service = Nothing
            Catch ex As Exception
                service.Abort()
            End Try
        End If

    End Sub
#End Region
    Private ReadOnly Property CommunitiesNews(ByVal PersonID As Integer) As List(Of lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount)
        Get
            Dim oList As List(Of lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount)
            If _CommunitiesNews Is Nothing Then
                _CommunitiesNews = New Dictionary(Of Integer, List(Of lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount))
            End If
            If _CommunitiesNews.ContainsKey(PersonID) Then
                oList = _CommunitiesNews.Item(PersonID)
            Else
                oList = Me.NotificationManager.GetCommunityNewsCount(PersonID)
                Try
                    _CommunitiesNews.Add(PersonID, oList)
                Catch ex As Exception

                End Try
            End If
            Return oList
        End Get
    End Property
    Public Property NotificationManager() As lm.Modules.NotificationSystem.Business.ManagerCommunitynews
        Get
            If IsNothing(_NotificationManager) Then
                _NotificationManager = New lm.Modules.NotificationSystem.Business.ManagerCommunitynews(CurrentContext)
            End If
            Return _NotificationManager
        End Get
        Set(ByVal value As lm.Modules.NotificationSystem.Business.ManagerCommunitynews)
            _NotificationManager = value
        End Set
    End Property
#End Region

#Region "Service Action"
    Private ReadOnly Property TrapProgressive As Long
        Get

            Dim progr As Int64 = 0

            Try
                If TypeOf _Application.Item("TrapProgressive") Is Long Then
                    progr = _Application.Item("TrapProgressive")
                End If
            Catch ex As Exception

            End Try

            progr += 1
            _Application.Item("TrapProgressive") = progr

            Return progr

        End Get
    End Property


    Public Sub AddLoginAction()
        SendTrapLogin()
        'If AllowSendTrapActions AndAlso Not IsNothing(TrapSender) Then
        '    Dim actionvalue As WsSnmtp.dtoActionValues = New WsSnmtp.dtoActionValues()
        '    With actionvalue
        '        .Progressive = TrapProgressive
        '        .User = New WsSnmtp.dtoUserValues()
        '        .Action = New WsSnmtp.dtoActionData()
        '    End With

        '    With actionvalue.User
        '        .id = Me.CurrentUser.ID
        '        .login = Me.CurrentUser.Login
        '        .mail = Me.CurrentUser.Mail
        '        .name = Me.CurrentUser.Nome
        '        .surname = Me.CurrentUser.Cognome
        '        .taxCode = Me.CurrentUser.CodFiscale
        '    End With

        '    With actionvalue.Action
        '        .ActionCodeId = "login"
        '        .ActionTypeId = ""
        '        .CommunityId = Me.WorkingCommunityID
        '        .CommunityIsFederated = False
        '        .InteractionType = InteractionType.Generic
        '        .ModuleCode = ""
        '        .ModuleId = CurrentModule.ID
        '        .ObjectId = 0
        '        .ObjectType = 0
        '    End With

        '    Dim TrapId As Integer = TrapIdEnums.LoginSuccess
        '    TrapSender.SendTrapActionValue(TrapId, actionvalue)
        'End If

        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(Me.WorkingCommunityID, CurrentModule.ID, 0, InteractionType.Generic, Nothing)
                    If Not IsNothing(oAction) AndAlso Me.SystemSettings.ActionService.EnableAction Then
                        Me.RegisterAction.OpenWorkingSession(oAction)
                    End If
                    If Me.SystemSettings.ActionService.EnableBrowser AndAlso Not IsNothing(oAction) Then
                        Dim oBrowser As WS_Actions.BrowserInfo = CreateBrowser(HttpContext.Current)
                        oBrowser.PersonID = Me.CurrentUser.ID
                        oBrowser.PersonTypeID = Me.CurrentUser.TipoPersona.ID
                        oBrowser.WorkingSessionID = oAction.WorkingSessionID
                        oBrowser.ClientIPAdress = oAction.ClientIPadress
                        oBrowser.ProxyIPAdress = oAction.ProxyIPadress
                        Me.RegisterAction.AddBrowserInfo(oBrowser)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub LogoutAction()
        SendTrapLogout()

        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled AndAlso Me.SystemSettings.ActionService.EnableAction Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(Me.WorkingCommunityID, CurrentModule.ID, 0, InteractionType.Generic, Nothing)
                    If Not IsNothing(oAction) Then
                        Me.RegisterAction.CloseWorkingSession(oAction)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub AddAction(ByVal oType As Integer, Optional ByVal oObjectActions As List(Of WS_Actions.ObjectAction) = Nothing, Optional ByVal oTypeIteration As InteractionType = InteractionType.Generic)
        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled AndAlso Me.SystemSettings.ActionService.EnableAction Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(Me.WorkingCommunityID, CurrentModule.ID, oType, oTypeIteration, oObjectActions)
                    If Not IsNothing(oAction) Then
                        Me.RegisterAction.AddAction(oAction)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub AddAction(ByVal CommunityID As Integer, ByVal oType As Integer, Optional ByVal oObjectActions As List(Of WS_Actions.ObjectAction) = Nothing, Optional ByVal oTypeIteration As InteractionType = InteractionType.Generic)
        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled AndAlso Me.SystemSettings.ActionService.EnableAction Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(CommunityID, CurrentModule.ID, oType, oTypeIteration, oObjectActions)
                    If Not IsNothing(oAction) Then
                        Me.RegisterAction.AddAction(oAction)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub AddActionToModule(ByVal ModuleID As Integer, ByVal oType As Integer, Optional ByVal oObjectActions As List(Of WS_Actions.ObjectAction) = Nothing, Optional ByVal oTypeIteration As InteractionType = InteractionType.Generic)
        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled AndAlso Me.SystemSettings.ActionService.EnableAction Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(Me.WorkingCommunityID, ModuleID, oType, oTypeIteration, oObjectActions)
                    If Not IsNothing(oAction) Then
                        Me.RegisterAction.AddAction(oAction)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub AddActionToModule(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oType As Integer, Optional ByVal oObjectActions As List(Of WS_Actions.ObjectAction) = Nothing, Optional ByVal oTypeIteration As InteractionType = InteractionType.Generic)
        If AllowRegisterActions Then
            If IsNothing(Me.RegisterAction) Then
                Exit Sub
            ElseIf Me.SystemSettings.ActionService.Enabled AndAlso Me.SystemSettings.ActionService.EnableAction Then
                Try
                    Dim oAction As WS_Actions.UserAction = CreateActionToService(CommunityID, ModuleID, oType, oTypeIteration, oObjectActions)
                    If Not IsNothing(oAction) Then
                        Me.RegisterAction.AddAction(oAction)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub

    Private Function CreateActionToService(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oActionType As Integer, ByVal oInteraction As InteractionType, ByVal oObjectActions As List(Of WS_Actions.ObjectAction)) As WS_Actions.UserAction
        Dim oAction As WS_Actions.UserAction

        Try
            Dim WorkingSessionID As System.Guid = Me.UniqueGuidSession
            If WorkingSessionID = System.Guid.Empty Then
                WorkingSessionID = System.Guid.NewGuid
                Me.UniqueGuidSession = WorkingSessionID
            End If

            oAction = New WS_Actions.UserAction
            With oAction
                .ID = System.Guid.NewGuid
                .ActionDate = Now
                .ClientIPadress = ClientIPadress()
                .ProxyIPadress = ProxyIPadress()
                .CommunityID = CommunityID
                .Interaction = oInteraction
                .ModuleID = ModuleID
                .PersonID = Me.CurrentUser.ID
                .Type = oActionType
                If Not IsNothing(oObjectActions) Then
                    .ObjectActions = oObjectActions.ToArray
                End If
                .WorkingSessionID = WorkingSessionID
                If Me.isPortalCommunity Then
                    .PersonRoleID = DefaultRoleID
                Else
                    .PersonRoleID = CurrentRoleID
                End If
            End With
        Catch ex As Exception

        End Try
        Return oAction
    End Function
    Public Function CreateObjectsList(ByVal oType As Integer, ByVal oValueID As String) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList.Add(New WS_Actions.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = Me.CurrentModule.ID})
        Return oList
    End Function
    Public Function CreateObjectsList(ByVal ModuleID As Integer, ByVal oType As Integer, ByVal oValueID As String) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList.Add(New WS_Actions.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = ModuleID})
        Return oList
    End Function
    Public Function CreateObjectsList(ByVal idModule As Integer, ByVal oType As Integer, ByVal idItems As List(Of Integer)) As List(Of WS_Actions.ObjectAction)
        Return idItems.Select(Function(i) New WS_Actions.ObjectAction With {.ObjectTypeId = oType, .ValueID = i, .ModuleID = idModule}).ToList()
    End Function
    Public Function CreateObjectAction(ByVal oType As Integer, ByVal oValueID As String) As WS_Actions.ObjectAction
        Return New WS_Actions.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = Me.CurrentModule.ID}
    End Function
    Public Function CreateObjectAction(ByVal ModuleID As Integer, ByVal oType As Integer, ByVal oValueID As String) As WS_Actions.ObjectAction
        Return New WS_Actions.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = ModuleID}
    End Function
    Public Function CreateObjectsList(idModule As Integer, items As Dictionary(Of Integer, String)) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        For Each item In items
            oList.Add(New WS_Actions.ObjectAction With {.ObjectTypeId = item.Key, .ValueID = item.Value, .ModuleID = idModule})
        Next

        Return oList
    End Function
    Public Function CreateObjectsList(idModule As Integer, items As Dictionary(Of Integer, List(Of Long))) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        For Each item In items
            oList.AddRange(item.Value.Select(Function(i) New WS_Actions.ObjectAction With {.ObjectTypeId = item.Key, .ValueID = i.ToString, .ModuleID = idModule}).ToList())
        Next

        Return oList
    End Function
    Private Shared Function CreateBrowser(ByVal oContext As System.Web.HttpContext) As WS_Actions.BrowserInfo
        Dim oBrowserInfo As New WS_Actions.BrowserInfo
        Dim oCapabilities As System.Web.HttpBrowserCapabilities = oContext.Request.Browser
        With oBrowserInfo
            .ActiveXControls = oCapabilities.ActiveXControls
            .CanInitiateVoiceCall = oCapabilities.CanInitiateVoiceCall
            .Cookies = oCapabilities.Cookies
            .Frames = oCapabilities.Frames
            .IsMobileDevice = oCapabilities.IsMobileDevice
            .JavaApplets = oCapabilities.JavaApplets
            .JScriptVersion = oCapabilities.JScriptVersion.Major
            .Platform = oCapabilities.Platform
            .ScreenCharactersWidth = oCapabilities.ScreenCharactersWidth
            .ScreenPixelsHeight = oCapabilities.ScreenCharactersHeight
            .Tables = oCapabilities.Tables
            .Version = oCapabilities.Version
            .W3CDomVersion = oCapabilities.W3CDomVersion.Minor
            .BrowserType = oCapabilities.Browser
            If oContext.Request.UserLanguages.Length = 0 Then
                .Language = ""
            Else
                .Language = oContext.Request.UserLanguages(0)
            End If
        End With
        Return oBrowserInfo
    End Function
#End Region

#End Region

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

    Public Sub New(ByVal oHttpContext As HttpContext, Optional ByVal oActionService As iRemoteService = Nothing)
        _Request = oHttpContext.Request
        _Server = oHttpContext.Server
        _Response = oHttpContext.Response
        _Session = oHttpContext.Session
        _Application = oHttpContext.Application
        _Context = oHttpContext
        _ActionService = oActionService
    End Sub


#Region "ApplicationContext"
    Public Shared ApplicationWorkingId As System.Guid
    Public Property IsInDebugMode() As Boolean
        Get
            Dim _IsInDebugMode As Boolean = False
            If Not IsNothing(_Session) Then
                Boolean.TryParse(_Session("IsInDebugMode"), _IsInDebugMode)
            End If
            Return _IsInDebugMode
        End Get
        Set(ByVal value As Boolean)
            If Not IsNothing(_Session) Then
                _Session("ClientIPadress") = "127.0.0.1"
                _Session("ProxyIPadress") = "127.0.0.1"
                _Session("IsInDebugMode") = value
                _Session("AllowRegisterActions") = Not value
            End If
        End Set
    End Property
    Public ReadOnly Property AllowRegisterActions() As Boolean
        Get
            If Not _AllowRegisterActions.HasValue Then
                Dim allow As Boolean = True
                If Not IsNothing(_Session) Then
                    If IsNothing(_Session("AllowRegisterActions")) Then
                        _Session("AllowRegisterActions") = allow
                    ElseIf Not Boolean.TryParse(_Session("AllowRegisterActions"), allow) Then
                        allow = True
                        _Session("AllowRegisterActions") = allow
                    End If
                End If
                _AllowRegisterActions = allow
            End If
            Return _AllowRegisterActions.Value
        End Get
    End Property
    Public ReadOnly Property AllowSendTrapActions() As Boolean
        Get

            Dim allow As Boolean = False

            Try
                If TypeOf _Application.Item("SendTrapActionsAllowed") Is Boolean Then
                    Return _Application.Item("SendTrapActionsAllowed")
                End If

                allow = System.Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings("SendTrapActionsAllowed"))
            Catch ex As Exception

            End Try

            _Application.Item("SendTrapActionsAllowed") = allow

            Return allow
        End Get
    End Property
    Public ReadOnly Property AccessAvailable() As Boolean
        Get
            Try
                If TypeOf _Application.Item("SystemAcess") Is Boolean Then
                    Return _Application.Item("SystemAcess")
                Else
                    Return False
                End If
            Catch ex As Exception

            End Try
        End Get
        'Set(ByVal value As Boolean)
        '    _Application.Lock()
        '    _Application.Item("SystemAcess") = value
        '    _Application.UnLock()
        'End Set
    End Property
    Public ReadOnly Property BaseUrlDrivePath() As String
        Get
            Return _Server.MapPath(Me.BaseUrl)
        End Get
    End Property
    Public ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
    Public ReadOnly Property BaseUrl() As String
        Get
            Return BaseUrl(False)
        End Get
    End Property
    Public ReadOnly Property SecureBaseUrl() As String
        Get
            Return BaseUrl(True)
        End Get
    End Property
    Public ReadOnly Property BaseUrl(ByVal secure As Boolean) As String
        Get
            If secure Then
                If _SecureBaseUrl = "" Then
                    Dim url As String = Me.SecureApplicationUrlBase
                    If url.EndsWith("/") Then
                        _SecureBaseUrl = url
                    Else
                        _SecureBaseUrl = url + "/"
                    End If
                End If

                Return _SecureBaseUrl
            Else
                If _BaseUrl = "" Then
                    Dim url As String = Me._Request.ApplicationPath
                    If url.EndsWith("/") Then
                        _BaseUrl = url
                    Else
                        _BaseUrl = url + "/"
                    End If
                End If

                Return _BaseUrl
            End If
        End Get
    End Property
    'Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False) As String
    '    Get
    '        Dim Redirect As String = "http"

    '        If RequireSSL And Not WithoutSSLfromConfig Then
    '            Redirect &= "s://" & Me._Request.Url.Host & Me.BaseUrl
    '        Else
    '            Redirect &= "://" & Me._Request.Url.Host & Me.BaseUrl
    '        End If
    '        ApplicationUrlBase = Redirect
    '    End Get
    'End Property
    Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False, Optional ByVal forLoginPage As Boolean = False) As String
        Get
            Dim Redirect As String = "http"

            If (RequireSSL AndAlso Not WithoutSSLfromConfig) OrElse (forLoginPage AndAlso SystemSettings.Login.isSSLloginRequired) Then
                Redirect &= "s://" & Me._Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me._Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property
    Public ReadOnly Property ProfilePath() As String
        Get
            Return PhysicalApplicationPath & "Profili\"
        End Get
    End Property
    Public ReadOnly Property PhysicalApplicationPath() As String
        Get
            If Not IsNothing(_Request) Then
                Return _Request.PhysicalApplicationPath
            Else
                Return ""
            End If
        End Get
    End Property


    Public ReadOnly Property SecureApplicationUrlBase() As String
        Get
            Return "https://" & Me._Request.Url.Host & Me.BaseUrl
        End Get
    End Property
    Public ReadOnly Property DefaultUrl() As String
        Get
            If (String.IsNullOrEmpty(SystemSettings.Presenter.FullDefaultStartPage)) Then
                Return (Me.ApplicationUrlBase & Me.SystemSettings.Presenter.DefaultStartPage)
            Else
                Return SystemSettings.Presenter.FullDefaultStartPage
            End If
        End Get
    End Property
    Public ReadOnly Property LocalizedMail() As MailLocalized
        Get
            Try
                Return ManagerConfiguration.GetMailLocalized(Me.UserSessionLanguage)
                'If LanguageCode = "" Then
                '	Return SystemSettings.Mail.Localized(SystemConfiguration.GetLocalizedConfigurations(Me.LinguaCode))
                'Else
                '	Return SystemSettings.Mail.Localized(SystemConfiguration.GetLocalizedConfigurations(LanguageCode))
                'End If

            Catch ex As Exception

            End Try
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property LocalizedMail(ByVal IdLanguage As Integer) As MailLocalized
        Get
            Try
                Return ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByID(IdLanguage))
            Catch ex As Exception

            End Try
            Return Nothing
        End Get
    End Property
    Public Shared Function ClientIPadress() As String
        If Not TypeOf HttpContext.Current.Session("ClientIPadress") Is String Then
            HttpContext.Current.Session("ClientIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ClientAddress
        ElseIf String.IsNullOrEmpty(HttpContext.Current.Session("ClientIPadress")) Then
            HttpContext.Current.Session("ClientIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ClientAddress
        End If
        Return HttpContext.Current.Session("ClientIPadress")
    End Function
    Public Shared Function ProxyIPadress() As String
        If Not TypeOf HttpContext.Current.Session("ProxyIPadress") Is String Then
            HttpContext.Current.Session("ProxyIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ProxyAddress
        ElseIf String.IsNullOrEmpty(HttpContext.Current.Session("ClientIPadress")) Then
            HttpContext.Current.Session("ProxyIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ProxyAddress
        End If
        Return HttpContext.Current.Session("ProxyIPadress")
    End Function

    Public Function GetResourceConfig(ByVal LinguaCode As String) As ResourceManager
        Dim oResourceConfig = New ResourceManager

        If LinguaCode = "" Then
            LinguaCode = "it-IT"
        End If
        oResourceConfig.UserLanguages = LinguaCode
        oResourceConfig.ResourcesName = System.Configuration.ConfigurationManager.AppSettings("configFile")
        oResourceConfig.Folder_Level1 = "Root"

        oResourceConfig.setCulture()
        Return oResourceConfig
    End Function
    Public ReadOnly Property IstituzioneID() As Integer
        Get
            Try
                IstituzioneID = _Session("ISTT_ID")
            Catch ex As Exception
                IstituzioneID = 0
            End Try
        End Get
    End Property
#End Region

#Region "UserContext"
    Public ReadOnly Property IsoLanguageCode As String
        Get
            If IsNothing(_Session("IsoLanguageCode")) OrElse String.IsNullOrEmpty(_Session("IsoLanguageCode")) Then
                _Session("IsoLanguageCode") = New System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID, False).TwoLetterISOLanguageName
            End If
            Return _Session("IsoLanguageCode")
        End Get
    End Property
    Public ReadOnly Property IsoLanguageCodeChanged As String
        Get
            Dim isChanged As Boolean
            Boolean.TryParse(_Session("IsoLanguageCodeChanged"), isChanged)
            _Session("IsoLanguageCodeChanged") = isChanged
            Return isChanged
        End Get
    End Property


    Public ReadOnly Property AnonymousPerson() As Person
        Get
            Dim oUser As Person = System.Web.HttpContext.Current.Application("AnonymousPerson")
            If IsNothing(oUser) Then
                oUser = GetAnonymousUser()
                System.Web.HttpContext.Current.Application("AnonymousPerson") = oUser
            End If
            Return oUser
        End Get
    End Property
    Public ReadOnly Property AnonymousCOL_Persona() As COL_Persona
        Get
            Dim oUser As COL_Persona = System.Web.HttpContext.Current.Application("AnonymousCOL_Persona")
            If IsNothing(oUser) Then
                System.Web.HttpContext.Current.Application("AnonymousCOL_Persona") = GetAnonymousCOL_Persona()
                oUser = System.Web.HttpContext.Current.Application("AnonymousCOL_Persona")
            End If
            Return oUser
        End Get
    End Property
    Private Function GetAnonymousUser() As Person
        Dim oPersona As New COL_Persona
        oPersona = GetAnonymousCOL_Persona()
        Return New Person(oPersona.ID, oPersona.Nome, oPersona.Cognome)
    End Function
    Private Function GetAnonymousCOL_Persona() As COL_Persona
        Dim oPersona As New COL_Persona
        Dim oLingua As New Lingua

        Try
            If String.IsNullOrEmpty(Me._Context.Session("LinguaCode")) OrElse Not IsNumeric(Me._Context.Session("LinguaID")) Then
                oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
            Else
                oLingua = New Lingua(Me._Context.Session("LinguaID"), Me._Context.Session("LinguaCode"))
            End If
        Catch ex As Exception
            oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
        End Try
        Return COL_Persona.GetUtenteAnonimo(oLingua)
    End Function

    'Public Property UserDefaultOrganizationId() As Integer
    '    Get
    '        Try
    '            If Not IsNumeric(_Session("ORGN_id")) Then
    '                _Session("ORGN_id") = 0
    '            End If
    '        Catch ex As Exception
    '            _Session("ORGN_id") = 0
    '        End Try
    '        UserDefaultOrganizationId = DirectCast(_Session("ORGN_id"), Integer)
    '    End Get
    '    Set(ByVal value As Integer)
    '        _Context.Session("ORGN_id") = value
    '    End Set
    'End Property

    Public Function GetSkinIdOrganization() As Integer
        Dim idOrganization As Integer = 0
        If CurrentContext.UserContext.CurrentCommunityID > 0 Then
            idOrganization = UserCurrentIdOrganization
        Else
            idOrganization = UserDefaultIdOrganization
        End If
        Return idOrganization
    End Function

    Public Property UserDefaultIdOrganization() As Integer
        Get
            If IsNothing(_Session("UserDefaultIdOrganization")) AndAlso Not IsNumeric(_Session("UserDefaultIdOrganization")) Then
                _Session("UserDefaultIdOrganization") = BaseModuleManager.GetUserDefaultIdOrganization(CurrentContext.UserContext.CurrentUserID)
            End If
            UserCurrentIdOrganization = DirectCast(_Session("UserDefaultIdOrganization"), Integer)
        End Get
        Set(ByVal value As Integer)
            _Context.Session("UserDefaultIdOrganization") = value
        End Set
    End Property
    Public Property UserCurrentIdOrganization() As Integer
        Get
            Dim idOrganization As Integer = 0
            If IsNumeric(_Session("ORGN_id")) Then
                idOrganization = CInt(_Session("ORGN_id"))
            End If
            If idOrganization = 0 AndAlso CurrentContext.UserContext.CurrentCommunityID > 0 Then
                idOrganization = ComunitaCorrente.GetOrganizzazioneID()
                _Context.Session("ORGN_id") = idOrganization
            End If
            Return idOrganization
        End Get
        Set(ByVal value As Integer)
            _Context.Session("ORGN_id") = value
        End Set
    End Property
    Public ReadOnly Property CurrentRoleID() As Integer
        Get
            Try
                CurrentRoleID = _Session("IdRuolo")
            Catch ex As Exception
                CurrentRoleID = Main.TipoRuoloStandard.AccessoNonAutenticato
            End Try
        End Get
    End Property
    Public ReadOnly Property DefaultRoleID() As Integer
        Get
            Try
                If IsNothing(_Session("DefaultRoleID")) Then
                    _Session("DefaultRoleID") = ManagerPersona.GetDefaultRoleID(Me.CurrentUser.ID)
                End If
                Return _Session("DefaultRoleID")
            Catch ex As Exception
                DefaultRoleID = Main.TipoRuoloStandard.AccessoNonAutenticato
            End Try
        End Get
    End Property

    Public ReadOnly Property UserSessionLanguage() As Lingua
        Get
            Return New Lingua(Me.LinguaID, Me.LinguaCode)
        End Get
    End Property
    Public Property LinguaCode() As String
        Get
            Dim CodeLingua As String = ""
            Try
                CodeLingua = HttpContext.Current.Session("LinguaCode")
            Catch ex As Exception

            End Try
            If CodeLingua = "" Then
                Dim oLingua As Lingua = ManagerLingua.GetDefault
                If Not IsNothing(oLingua) Then
                    CodeLingua = oLingua.Codice
                    HttpContext.Current.Session("LinguaID") = oLingua.ID
                End If
            End If
            Return CodeLingua
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("LinguaCode") = value
        End Set
    End Property
    Public Property LinguaID() As Integer
        Get
            Dim Codice As Integer = 0
            Try
                Codice = CInt(HttpContext.Current.Session("LinguaID"))
            Catch ex As Exception
            End Try
            If Codice = 0 Then
                Dim oLingua As Lingua = ManagerLingua.GetDefault
                If Not IsNothing(oLingua) Then
                    Codice = oLingua.ID
                    HttpContext.Current.Session("LinguaCode") = oLingua.Codice
                End If
            End If
            Return Codice
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session("LinguaID") = value
        End Set
    End Property
    Protected Property NewLinguaID() As Integer
        Get
            Try
                If IsNumeric(_Session("NewLinguaID")) Then
                    NewLinguaID = CInt(_Session("NewLinguaID"))
                Else
                    NewLinguaID = 0
                End If
            Catch ex As Exception
                NewLinguaID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            _Session("NewLinguaID") = value
        End Set
    End Property
    Public Property PreloggedUserId As Integer
        Get
            Dim idUser As Integer = 0
            If IsNumeric(Me._Context.Session("PreloggedUserId")) Then
                idUser = CInt(Me._Context.Session("PreloggedUserId"))
            End If
            Return idUser
        End Get
        Set(value As Integer)
            Me._Context.Session("PreloggedUserId") = value
        End Set
    End Property
    Public Property PreloggedProviderId As Long
        Get
            Dim idProvider As Long = 0
            Long.TryParse(Me._Context.Session("PreloggedUserId"), idProvider)
            Return idProvider
        End Get
        Set(value As Long)
            Me._Context.Session("PreloggedProviderId") = value
        End Set
    End Property
    Public Property PreloggedProviderUrl As String
        Get
            Return Me._Context.Session("PreloggedProviderUrl")
        End Get
        Set(value As String)
            Me._Context.Session("PreloggedProviderUrl") = value
        End Set
    End Property

    Public ReadOnly Property CurrentUser() As COL_Persona
        Get
            Try
                If TypeOf _Session("objPersona") Is COL_Persona Then
                    CurrentUser = _Session("objPersona")
                Else
                    Return Me.AnonymousCOL_Persona
                End If
            Catch ex As Exception
                Return Me.AnonymousCOL_Persona
            End Try
        End Get
    End Property


    Public Sub GenerateNewSession()
        If Not IsNothing(HttpContext.Current) Then
            With HttpContext.Current
                .Session("ISTT_ID") = 1
                .Session("LogonAs") = False
                .Session("CMNT_path_forAdmin") = ""
                .Session("CMNT_path_forNews") = ""
                .Session("OrgnIDtoSubscribe") = ""
                .Session("TipoPersonaIDtoSubscribe") = ""
                .Session("IstituzioneIDtoSubscribe") = ""
                .Session("oImpostazioni") = Nothing
                .Session("NewLanguageId") = 0

                .Session("limbo") = True
                .Session("objPersona") = Nothing
                .Session("ORGN_id") = 0
                .Session("Istituzione") = 1
                .Session("IsoLanguageCodeChanged") = False
                .Session("IdRuolo") = Main.TipoRuoloStandard.AccessoNonAutenticato
                .Session("IdComunita") = 0
                .Session("ArrPermessi") = ""
                .Session("ArrComunita") = ""
                .Session("RLPC_ID") = 0
                .Session("AdminForChange") = False
                .Session("CMNT_path_forAdmin") = ""
                .Session("idComunita_forAdmin") = ""
                .Session("DefaultRoleID") = Nothing
                Dim oLingua As New Lingua
                oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
                If Not IsNothing(oLingua) Then
                    LinguaCode = oLingua.Codice
                    LinguaID = oLingua.ID
                Else
                    LinguaCode = "it-IT"
                    LinguaID = 1
                End If

                ApiTokenClean()

                UniqueGuidSession = Guid.NewGuid
                Dim keys As List(Of String)
                If Not IsNothing(.Request) Then
                    keys = .Request.Cookies.AllKeys().Where(Function(i) i.StartsWith("temp_")).ToList
                    For Each key As String In keys
                        .Response.Cookies(key).Expires = DateTime.Now.AddDays(-1)
                        .Request.Cookies.Remove(key)
                    Next
                End If

            End With
        End If
    End Sub

    Public Property UniqueGuidSession() As System.Guid
        Get
            Dim iUniqueGuidSession As System.Guid
            If TypeOf HttpContext.Current.Session("UniqueGuidSession") Is System.Guid Then
                iUniqueGuidSession = HttpContext.Current.Session("UniqueGuidSession")
                If iUniqueGuidSession = Guid.Empty Then
                    iUniqueGuidSession = Guid.NewGuid
                    HttpContext.Current.Session("UniqueGuidSession") = iUniqueGuidSession
                End If
            Else
                iUniqueGuidSession = Guid.NewGuid
                HttpContext.Current.Session("UniqueGuidSession") = iUniqueGuidSession
            End If
            Return iUniqueGuidSession
        End Get
        Set(ByVal value As System.Guid)
            HttpContext.Current.Session("UniqueGuidSession") = value
        End Set
    End Property

    Private Sub SetCookies(ByVal IdLanguage As Integer, ByVal code As String)
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = _Request.Browser

        If oBrowser.Cookies Then
            Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", IdLanguage.ToString)
            Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", code)

            oCookie_ID.Expires = Now.AddYears(1)
            oCookie_Code.Expires = Now.AddYears(1)

            _Response.Cookies.Add(oCookie_ID)
            _Response.Cookies.Add(oCookie_Code)
        End If
    End Sub
    Private Sub WriteLoginCookie(user As lm.Comol.Core.DomainModel.Person, ByVal idProvider As Long, ByVal url As String)
        'Cookie Login Blog
        Dim secured As HttpCookie

        Dim userid = user.Id
        Dim username = user.Id

        Dim domain As String = "comol.local"
        Dim minutes As Long = 5
        Dim hash As New Hashtable()
        hash.Add("userId", userid)
        hash.Add("userName", username)
        hash.Add("expire", Now.AddMinutes(minutes))
        hash.Add("domain", domain)
        hash.Add("provider", idProvider)

        secured = SecuredCookie.encode_cookie("login", domain, minutes, hash)

        _Response.Cookies.Add(secured)
        WriteProviderCookie(user, idProvider, url)
    End Sub


    Private _TokenService As lm.Comol.Core.BaseModules.ApiToken.Business.TokenService

    Private ReadOnly Property TokenService As lm.Comol.Core.BaseModules.ApiToken.Business.TokenService
        Get
            If IsNothing(_TokenService) Then
                _TokenService = New lm.Comol.Core.BaseModules.ApiToken.Business.TokenService(Me.CurrentContext)
            End If

            Return _TokenService
        End Get
    End Property

    Public Sub ApiTokenClean()
        ApiTokenWriteCookie("", "", 0, 0, BaseUrl, 0, "")
    End Sub

    Public Sub ApiTokenRefresh(
                           userId As Integer,
                           comId As String,
                           LangId As Integer,
                           LangCode As String,
                           forceUpdate As Boolean
                            )

        'View.GetWorkingSessionId().ToString();  '  Return Me.PageUtility.UniqueGuidSession

        Dim DeviceId As String = Me.UniqueGuidSession.ToString()

        Dim token As String = TokenService.TokenRefresh(
            userId,
            DeviceId,
            lm.Comol.Core.BaseModules.ApiToken.Domain.TokenType.AdevicoWeb,
            forceUpdate)

        ApiTokenWriteCookie(
            token,
            Me.UniqueGuidSession.ToString,
            comId,
            userId,
            BaseUrl,
            LangId,
            LangCode)

    End Sub

    'Da testare!
    Public Sub ApiTokenWriteCookie(
                                     token As String,
                                     workingSessionId As String,
                                     comId As String,
                                     usrId As String,
                                     url As String,
                                     LanguageId As String,
                                     Languagecode As String)


        'If Not String.IsNullOrEmpty(Request.QueryString("ComId")) AndAlso IsNumeric(Request.QueryString("ComId")) Then
        '    Dim RequestComId As Integer = 0
        '    Try
        '        RequestComId = System.Convert.ToInt32(Request.QueryString("ComId"))
        '    Catch ex As Exception
        '    End Try
        '    If RequestComId > 0 Then
        '        comId = RequestComId.ToString()
        '    End If
        'End If

        SetResponseCookie(CookieHelper.CookieKeyToked, token, url)
        SetResponseCookie(CookieHelper.CookieKeyDeviceId, workingSessionId, url)
        SetResponseCookie(CookieHelper.CookieKeyCommunityId, comId, url)
        SetResponseCookie(CookieHelper.CookieKeyPersonId, usrId, url)
        SetResponseCookie(CookieHelper.CookieKeyLangId, LanguageId, url)
        SetResponseCookie(CookieHelper.CookieKeyLangCode, Languagecode, url)
    End Sub

    Public Sub ApiTokenSetCommunity(comId As String)
        SetResponseCookie(CookieHelper.CookieKeyCommunityId, comId, BaseUrl)
    End Sub


    Public Function ApiTokenGetCommunity() As Integer

        Dim ComId As Integer = 0


        If Me.ComunitaCorrenteID > 0 Then
            ComId = Me.ComunitaCorrenteID
        Else

            Try
                Dim CookieComId As String = _Request.Cookies.Get(CookieHelper.CookieKeyCommunityId).Value

                If IsNumeric(CookieComId) Then
                    ComId = System.Convert.ToInt32(CookieComId)
                End If

            Catch ex As Exception

            End Try
        End If

        Return ComId
    End Function



    Private Sub WriteApiWrapperTokenCookieCom(comId As String, url As String)
        SetResponseCookie(CookieHelper.CookieKeyCommunityId, comId, url)
    End Sub


    'Da testare!
    Private Sub SetResponseCookie(
                                 ByVal key As String,
                                 ByVal value As String,
                                 ByVal url As String)

        Dim myCookie As HttpCookie = New HttpCookie(key, value)
        myCookie.Expires = DateTime.Now.AddDays(1)
        Dim myurl As String = url
        If myurl.IndexOf("://localhost") > 0 Then
            myCookie.Domain = ""
            myCookie.Path = "/"
        End If

        _Response.Cookies.Add(myCookie)
    End Sub


    Private Sub WriteProviderCookie(user As lm.Comol.Core.DomainModel.Person, ByVal idProvider As Long, ByVal url As String)
        Dim cookie As New HttpCookie("Provider")
        cookie.Expires = DateTime.Now().AddHours(24)
        cookie.Values.Add("IdPerson", user.Id.ToString())
        cookie.Values.Add("IdProvider", idProvider.ToString())
        cookie.Values.Add("IdDefaultProvider", user.IdDefaultProvider.ToString())
        cookie.Values.Add("AuthenticationTypeID", user.AuthenticationTypeID.ToString())
        cookie.Values.Add("DefaultUrl", url)
        cookie.Values.Add("GetType", GetType(lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie).ToString)
        _Response.Cookies.Add(cookie)
    End Sub
    Private Function ReadLogoutAccessCookie(ByVal IdUser As Integer, ByVal PersonLogin As String) As dtoLogoutAccess
        Dim iResponse As New dtoLogoutAccess
        Dim oValues As New Hashtable
        Try
            For Each key As String In _Request.Cookies("LogoutAccess").Values.Keys
                oValues(key) = _Request.Cookies("LogoutAccess").Values(key)
            Next
            '    SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            iResponse.CommunityID = oValues.Item("CommunityID")
            iResponse.PersonID = oValues.Item("PersonID")
            ' iResponse.PersonLogin = oValues.Item("PersonLogin")
            iResponse.isDownloading = oValues.Item("Download")
            'If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = IdUser AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
            If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = IdUser)) OrElse iResponse.PersonID <= 0 Then
                If Not String.IsNullOrEmpty(oValues.Item("PostPage")) Then
                    iResponse.PageUrl = Replace(oValues.Item("PostPage"), "#_#", "&")
                End If
            Else
                iResponse.PageUrl = ""
            End If
        End If
        Return iResponse
    End Function
    Public Function ReadLogoutAccessCookie() As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl
        Dim item As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = Nothing
        Dim values As New Hashtable
        Try
            For Each key As String In _Request.Cookies("LogoutAccess").Values.Keys
                values(key) = _Request.Cookies("LogoutAccess").Values(key)
            Next
        Catch ex As Exception

        End Try
        If Not IsNothing(values) Then
            If values.Item("GetType") = "lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl" Then
                item = New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl
                With item
                    .Display = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode).GetByString(values.Item("Display"), lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None)
                    .CodeLanguage = values.Item("CodeLanguage")
                    If IsNumeric(values.Item("IdLanguage")) Then
                        .IdLanguage = CInt(values.Item("IdLanguage"))
                    End If
                    If IsNumeric(values.Item("IdCommunity")) Then
                        .IdCommunity = CInt(values.Item("IdCommunity"))
                    End If
                    If IsNumeric(values.Item("IdPerson")) Then
                        .IdPerson = CInt(values.Item("IdPerson"))
                    End If
                    Try
                        .Preserve = CBool(values.Item("Preserve"))
                    Catch ex As Exception

                    End Try
                    .DestinationUrl = values.Item("DestinationUrl")
                    If Not String.IsNullOrEmpty(.DestinationUrl) Then
                        .DestinationUrl = .DestinationUrl.Replace("#_#", "&")
                    End If
                    Try
                        .IsForDownload = CBool(values.Item("IsForDownload"))
                    Catch ex As Exception

                    End Try
                    .PreviousUrl = values.Item("PreviousUrl")
                    If Not String.IsNullOrEmpty(.PreviousUrl) Then
                        .PreviousUrl = .PreviousUrl.Replace("#_#", "&")
                    End If
                    Try
                        .SendToHomeDashboard = CBool(values.Item("SendToHomeDashboard"))
                    Catch ex As Exception

                    End Try
                End With
                If String.IsNullOrEmpty(item.DestinationUrl) Then
                    ClearLogoutAccessCookie()
                    item = Nothing
                End If
            End If
        End If
        Return item
    End Function
    Public Function ReadLoginProviderCookie() As lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie
        Dim item As New lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie
        Dim values As New Hashtable
        Try
            For Each key As String In _Request.Cookies("Provider").Values.Keys
                values(key) = _Request.Cookies("Provider").Values(key)
            Next
        Catch ex As Exception

        End Try
        If Not IsNothing(values) Then
            If values.Item("GetType") = GetType(lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie).ToString Then
                Integer.TryParse(values.Item("IdPerson"), item.IdPerson)
                Integer.TryParse(values.Item("AuthenticationTypeID"), item.AuthenticationTypeID)
                Integer.TryParse(values.Item("IdDefaultProvider"), item.IdDefaultProvider)
                item.DefaultUrl = values.Item("DefaultUrl")
                Long.TryParse(values.Item("IdProvider"), item.IdProvider)
            End If
        End If
        Return item
    End Function
    Public Sub ClearLogonProviderCookie()
        _Response.Cookies("Provider").Expires = Now.AddDays(-1)
    End Sub
    Public Function ReadAutoOpenWindowCookie() As Boolean
        Dim result As Boolean = False
        Dim values As New Hashtable
        If Not IsNothing(_Request.Cookies("comol_AutoOpenWindow")) Then
            Try
                For Each key As String In _Request.Cookies("comol_AutoOpenWindow").Values.Keys
                    values(key) = _Request.Cookies("comol_AutoOpenWindow").Values(key)
                Next
            Catch ex As Exception

            End Try
            If Not IsNothing(values) Then
                If values.ContainsKey("AutoOpen") Then
                    result = values.Item("AutoOpen")
                End If
            End If
        End If

        Return result
    End Function
    Public Sub WriteAutoOpenWindowCookie(value As Boolean)
        Dim cookie As New HttpCookie("comol_AutoOpenWindow")
        cookie.Expires = DateTime.Now().AddHours(2)
        cookie.Values.Add("AutoOpen", value)
        _Response.Cookies.Add(cookie)
    End Sub
    Public Sub ClearAutoOpenWindowCookie()
        _Response.Cookies("comol_AutoOpenWindow").Expires = Now.AddDays(-1)
    End Sub
    Public Function GetDefaultLogoutPage() As String
        Dim item As lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie = ReadLoginProviderCookie()
        Dim resultUrl As String = ""

        If IsNothing(item) OrElse String.IsNullOrEmpty(item.DefaultUrl) Then
            resultUrl = ApplicationUrlBase(, True) & Me.SystemSettings.Presenter.DefaultStartPage
            '        Return IIf(BaseUrl = "/", "", BaseUrl) & Me.SystemSettings.Presenter.DefaultStartPage
        ElseIf Me.SystemSettings.Presenter.DefaultStartPage.Contains("elle3.aspx") Then
            resultUrl = ApplicationUrlBase(, True) & Me.SystemSettings.Presenter.DefaultStartPage
        Else
            resultUrl = item.DefaultUrl
            '        If Me.SystemSettings.Presenter.DefaultStartPage.Contains("elle3.aspx") Then
            '            Return IIf(BaseUrl = "/", "", BaseUrl) & Me.SystemSettings.Presenter.DefaultStartPage
            '        Else
            '            Return item.DefaultUrl
            '        End If
        End If
        Return resultUrl
    End Function
    Public Sub RedirectToDefaultLogoutPage(ByVal defaultLogonPage As String)
        Dim url As String = GetDefaultLogoutPage()

        '    Dim url As String = GetDefaultLogoutPage()
        If String.IsNullOrEmpty(url) Then
            url = defaultLogonPage
        End If

        If url.StartsWith("http") Then
            _Response.Redirect(url, True)
        Else
            Me.RedirectToUrl(url, SystemSettings.Login.isSSLloginRequired)
        End If
    End Sub


    Public Sub ClearLogoutAccessCookie()
        _Response.Cookies("LogoutAccess").Expires = Now.AddDays(-1)
    End Sub
#End Region

#Region "Community Context"
    Protected ReadOnly Property ComunitaCorrenteID() As Integer
        Get
            Try
                ComunitaCorrenteID = _Session("idComunita")
            Catch ex As Exception
                ComunitaCorrenteID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property WorkingCommunityID() As Integer
        Get
            If Me.isModalitaAmministrazione Then
                Return Me.AmministrazioneComunitaID
            Else
                Return Me.ComunitaCorrenteID
            End If
        End Get
    End Property
    Public ReadOnly Property ComunitaCorrente() As COL_Comunita
        Get
            If WorkingCommunityID > 0 Then
                Dim oComunita As New COL_Comunita
                oComunita.Id = Me.WorkingCommunityID

                If TypeOf (_Session("ComunitaCorrente")) Is COL_Comunita Then
                    If _Session("ComunitaCorrente").id <> Me.WorkingCommunityID Then
                        oComunita.EstraiByLingua(Me.LinguaID)
                        _Session("ComunitaCorrente") = oComunita
                    End If
                Else
                    oComunita.EstraiByLingua(Me.LinguaID)
                    _Session("ComunitaCorrente") = oComunita
                End If
                Return _Session("ComunitaCorrente")
            Else
                Return New COL_Comunita(0)
            End If
        End Get
    End Property
    Public Property AmministrazioneComunitaID() As Integer
        Get
            Try
                If Not IsNumeric(_Session("idComunita_forAdmin")) Then
                    _Session("idComunita_forAdmin") = 0
                End If
            Catch ex As Exception
                _Session("idComunita_forAdmin") = 0
            End Try
            AmministrazioneComunitaID = DirectCast(_Session("idComunita_forAdmin"), Integer)
        End Get
        Set(ByVal value As Integer)
            _Context.Session("idComunita_forAdmin") = value
        End Set
    End Property

    Public Property isModalitaAmministrazione() As Boolean
        Get
            Try
                If Not CBool(_Session("AdminForChange")) Then
                    _Session("AdminForChange") = False
                End If
            Catch ex As Exception
                _Session("AdminForChange") = False
            End Try
            isModalitaAmministrazione = _Session("AdminForChange")
        End Get
        Set(ByVal value As Boolean)
            _Session("AdminForChange") = value
        End Set
    End Property


    Public Function GetCurrentServices() As ServiziCorrenti
        If IsNothing(Me._ServiziCorrenti) OrElse _ServiziCorrenti.Count = 0 Then
            Me._ServiziCorrenti = New ServiziCorrenti
            For i As Integer = 0 To UBound(Me._Context.Session("ArrPermessi"), 1)
                Me._ServiziCorrenti.Add(Me._Context.Session("ArrPermessi")(i, 1), Me._Context.Session("ArrPermessi")(i, 0), Me._Context.Session("ArrPermessi")(i, 2))
            Next
        End If

        Return Me._ServiziCorrenti
    End Function
    Public Property isPortalCommunity() As Boolean
        Get
            Dim isPortale As Boolean = True
            Try
                isPortale = Me._Context.Session("Limbo")
            Catch ex As Exception

            End Try
            If Not isPortale AndAlso ComunitaCorrenteID = 0 Then
                isPortalCommunity = False
            Else
                isPortalCommunity = isPortale
            End If
        End Get
        Set(ByVal value As Boolean)
            Me._Context.Session("Limbo") = value
        End Set
    End Property
    Public Property CurrentModule() As PlainService
        Get
            If Not TypeOf Me._Context.Session("CurrentService") Is PlainService Then
                Me._Context.Session("CurrentService") = PlainService.Create(-1, "")
            End If
            CurrentModule = Me._Context.Session("CurrentService")
        End Get
        Set(ByVal value As PlainService)
            If IsNothing(value) Then
                value = PlainService.Create(-1, "")
            End If
            Me._Context.Session("CurrentService") = value
        End Set
    End Property
    Public ReadOnly Property GetModule(ByVal Code As String) As PlainService
        Get
            Dim oModule As PlainService = (From o In ManagerService.List Where o.Code = Code Select o).FirstOrDefault
            If IsNothing(oModule) Then : oModule = PlainService.Create(-1000, Code)
            End If
            Return oModule
        End Get
    End Property
    Public ReadOnly Property GetModuleID(ByVal Code As String) As Integer
        Get
            Return GetModule(Code).ID
        End Get
    End Property
#End Region

#Region "url Management"
    Public Sub RedirectToDefault(Optional ByVal QueryParameters As String = "")
        If QueryParameters = "" Then
            _Response.Redirect(Me.DefaultUrl, True)
        Else
            _Response.Redirect(Me.DefaultUrl & QueryParameters, True)
        End If
    End Sub
    Public Sub RedirectToUrl(ByVal Url As String)
        Dim Redirect As String = Url

        If Not Redirect.StartsWith("http") Then
            Redirect = String.Format(Me.ApplicationUrlBase & Url)
        End If

        _Response.Redirect(Redirect, True)
    End Sub
    Public Sub RedirectToSecureUrl(ByVal Url As String)
        Dim Redirect As String = Me.SecureApplicationUrlBase & Url

        _Response.Redirect(Redirect, True)
    End Sub
    Public Sub RedirectToUrl(ByVal Url As String, ByVal secure As Boolean)
        If secure Then
            RedirectToSecureUrl(Url)
        Else
            RedirectToUrl(Url)
        End If
    End Sub
    Public Sub RedirectToEncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType)
        _Response.Redirect(Me.EncryptedUrl(UrlPage, UrlQuerystring, oTypeEnc), True)
    End Sub
    Public Function EncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String) As String
        Return EncryptedUrl(UrlPage, UrlQuerystring, SecretKeyUtil.EncType.Altro)
    End Function
    Public Function EncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Redirect As String = "http"

        If RequireSSL Then
            Redirect &= "s://"
        Else
            Redirect &= "://"
        End If
        Redirect &= Me._Request.Url.Host & Me.BaseUrl & UrlPage & "?" & Me.CryptQuerystring(UrlQuerystring, oTypeEnc)

        Return Redirect
    End Function
    Public Function EncryptedQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(_Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function
    Public Function DecryptQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(_Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function

#Region "ArmoredQueryString"
    Public Function CryptQuerystring(ByVal data As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Return Enc.ArmorURL(data)
    End Function
    Private Function DecryptVerifyQuerystring(ByVal oTypeEnc As SecretKeyUtil.EncType) As Boolean
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Return Enc.DecryptVerifyURL(_Request.QueryString)
    End Function
#End Region

    Public Function GetUrlEncoded(ByVal url As String) As String
        'Return _Server.UrlEncode(url)
        Return HttpUtility.UrlEncode(url)
    End Function
    Public Function GetUrlDecoded(ByVal url As String) As String
        'Return _Server.UrlDecode(url)
        Return HttpUtility.UrlDecode(url)
    End Function
#End Region

#Region "Configuration File"
    Public ReadOnly Property CurrentSmtpConfig As lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig
        Get
            Dim SMTPsettings As New lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig()
            '  Dim mail As MailLocalized = Me.LocalizedMail
            With SMTPsettings
                .Authentication.Enabled = SystemSettings.Mail.AuthenticationEnabled AndAlso Not String.IsNullOrEmpty(SystemSettings.Mail.CredentialsUsername)
                .Authentication.EnableSsl = SystemSettings.Mail.UseSsl
                .Port = SystemSettings.Mail.HostPort
                If Not .Port > 0 Then
                    .Port = 25
                End If
                .MaxRecipients = SystemSettings.Presenter.DefaultSplitMailRecipients
                .Host = SystemSettings.Mail.ServerSMTP
                ' 
                .RelayAllowed = Not SystemSettings.Mail.SendMailByReply ' Not mail.SendMailByReply
                .SystemSender.DisplayName = SystemSettings.Mail.RealMailSenderAccount.DisplayName  ' mail.RealMailSenderAccount
                .SystemSender.Address = SystemSettings.Mail.RealMailSenderAccount.Address
                For Each l As Lingua In ManagerLingua.List
                    Dim oResourceConfig = New ResourceManager
                    oResourceConfig.UserLanguages = l.Codice
                    oResourceConfig.ResourcesName = Helpers.AppConfigSetting("LanguageSettingsFile")
                    oResourceConfig.Folder_Level1 = Helpers.AppConfigSetting("LanguageSettingsPath")
                    oResourceConfig.setCulture()

                    .DefaultSettings.Add(New lm.Comol.Core.MailCommons.Domain.Configurations.SenderSettings() With {.IdLanguage = l.ID, .CodeLanguage = l.Codice, .IsDefault = l.isDefault, .NoReplySignature = oResourceConfig.getValue("systemMailFirmaNotifica"), .Signature = oResourceConfig.getValue("systemMailFirma"), .SubjectForSenderCopy = oResourceConfig.getValue("copy"), .SubjectPrefix = oResourceConfig.getValue("systemMailSubject")})
                Next
            End With
            Return SMTPsettings
        End Get
    End Property
    Private ReadOnly Property ObjectPath(ByVal oPath As ConfigurationPath) As ObjectFilePath
        Get
            Dim oObjectPath As New ObjectFilePath(oPath.isOnThisServer)

            Try
                If oPath.isOnThisServer Then
                    oObjectPath.Virtual = Me.BaseUrl & oPath.VirtualPath
                    oObjectPath.Virtual = Replace(oObjectPath.Virtual, "//", "/")
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.DrivePath 'Me.BaseUrlDrivePath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = Me.BaseUrlDrivePath & oPath.VirtualPath
                    End If
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "/", "\")
                    oObjectPath.Drive = Replace(oObjectPath.Drive, "\\", "\")
                Else
                    If oPath.ServerVirtualPath <> "" Then
                        oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath
                    End If
                    If oPath.DrivePath <> "" Then
                        oObjectPath.Drive = oPath.ServerPath & oPath.DrivePath
                    Else
                        oObjectPath.Drive = oPath.ServerPath & oPath.VirtualPath
                    End If
                    oObjectPath.SharePath = oPath.ServerPath
                End If
                oObjectPath.RewritePath = oPath.RewritePath
            Catch ex As Exception

            End Try
            Return oObjectPath
        End Get
    End Property
    Public ReadOnly Property BaseUserRepositoryPath() As String
        Get
            Dim Path As String = ""
            If IsNothing(_FilePath) Then
                _BaseUserRepositoryPath = Me.ObjectPath(Me.SystemSettings.BaseFileRepositoryPath)
            End If
            Path = Replace(_BaseUserRepositoryPath.Drive, "\/", "\")
            Return Path
        End Get
    End Property
    Public ReadOnly Property FileDrivePath(Optional ByVal ComunitaID As Integer = -1) As String
        Get
            Dim Path As String = ""
            If IsNothing(_FilePath) Then
                _FilePath = Me.ObjectPath(Me.SystemSettings.File.Materiale)
            End If
            If ComunitaID <> -1 Then
                Path = _FilePath.Drive & ComunitaID & "\"
            Else
                Path = _FilePath.Drive
            End If
            Path = Replace(Path, "\/", "\")
            Return Path
        End Get
    End Property
    Public ReadOnly Property Configurations(ByVal ComunitaID As Integer) As Hashtable
        Get
            If IsNothing(_Configurations) Then
                _Configurations = New Hashtable
                _Configurations.Add(ConfigFileType.File, New Structure_FilePahConfiguration(ConfigFileType.File, Me.FileDrivePath(ComunitaID), Me.SystemSettings.File.Materiale))
                '_Configurations.Add(ConfigFileType.Scorm, New Structure_FilePahConfiguration(ConfigFileType.Scorm, ScormDrivePath(ComunitaID), Me.SystemSettings.File.Scorm))
                '_Configurations.Add(ConfigFileType.VideoCast, New Structure_FilePahConfiguration(ConfigFileType.VideoCast, VideoCastDrivePath(ComunitaID), Me.SystemSettings.File.VideoCast))
            End If
            Configurations = _Configurations
        End Get
    End Property
#End Region


#Region "NEW REPOSITORY PATH"
    Private _RepositoryDiskPath As String
    Public Function GetRepositoryDiskPath() As String
        If String.IsNullOrEmpty(_RepositoryDiskPath) Then
            If Not String.IsNullOrEmpty(SystemSettings.File.Materiale.DrivePath) Then
                _RepositoryDiskPath = SystemSettings.File.Materiale.DrivePath
            Else
                _RepositoryDiskPath = _Server.MapPath(BaseUrl & SystemSettings.File.Materiale.VirtualPath)
            End If
        End If
        Return _RepositoryDiskPath
    End Function
    Private _RepositoryThumbnailDiskPath As String
    Public Function GetRepositoryThumbnailDiskPath() As String
        If String.IsNullOrEmpty(_RepositoryThumbnailDiskPath) Then
            If Not String.IsNullOrEmpty(SystemSettings.File.FileThumbnail.DrivePath) Then
                _RepositoryThumbnailDiskPath = SystemSettings.File.FileThumbnail.DrivePath
            Else
                _RepositoryThumbnailDiskPath = _Server.MapPath(BaseUrl & SystemSettings.File.FileThumbnail.VirtualPath)
            End If
        End If
        Return _RepositoryThumbnailDiskPath
    End Function

#End Region

#Region "CommunityAccess"
    Private Function GetCommunityPath(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
        Dim oTreeComunita As New COL_TreeComunita
        Dim comPath As String = ""
        Try
            oTreeComunita.Directory = ProfilePath & PersonID & "\"
            oTreeComunita.Nome = PersonID & ".xml"
            Dim oPathList As List(Of CommunityPath) = oTreeComunita.FindCommunityPath(PersonID)

            comPath = (From p In oPathList Where p.ID = CommunityID Select p.Path).FirstOrDefault
            If comPath = "" Then
                oTreeComunita.AggiornaInfo(PersonID, LinguaID, -1, True)
                oPathList = oTreeComunita.FindCommunityPath(PersonID)

                comPath = (From p In oPathList Where p.ID = CommunityID Select p.Path).FirstOrDefault
            End If
        Catch ex As Exception

        End Try
        Return comPath
    End Function

    Public Function AccessToCommunity(
                                     ByVal idPerson As Integer,
                                     ByVal idCommunity As Integer,
                                     ByVal oResourceConfig As ResourceManager,
                                     ByVal UpdateAccessDate As Boolean) _
                                 As lm.Comol.Core.DomainModel.SubscriptionStatus

        Dim Path As String = GetCommunityPath(idPerson, idCommunity)
        Return AccessToCommunity(idPerson, idCommunity, Path, oResourceConfig, UpdateAccessDate)

    End Function

    Public Function AccessToCommunity(
                                     ByVal idPerson As Integer,
                                     ByVal idCommunity As Integer,
                                     path As String,
                                     ByVal oResourceConfig As ResourceManager,
                                     ByVal updateAccessDate As Boolean) _
                                 As lm.Comol.Core.DomainModel.SubscriptionStatus

        GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
        If path = "" Then
            Return lm.Comol.Core.DomainModel.SubscriptionStatus.none
        Else
            Return AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, "", updateAccessDate)
        End If
    End Function

    Public Function AccessToCommunity(
                                     ByVal PersonID As Integer,
                                     ByVal CommunityID As Integer,
                                     ByVal oResourceConfig As ResourceManager,
                                     ByVal LoadUrl As String,
                                     ByVal UpdateAccessDate As Boolean) _
                                 As lm.Comol.Core.DomainModel.SubscriptionStatus

        Dim Path As String = GetCommunityPath(PersonID, CommunityID)
        If Path = "" Then
            Return lm.Comol.Core.DomainModel.SubscriptionStatus.none
        Else
            Return AccessToCommunity(PersonID, CommunityID, Path, oResourceConfig, LoadUrl, UpdateAccessDate)
        End If

    End Function

    Public Function Federation(ByVal communityId As Integer) As lm.Comol.Core.BaseModules.Federation.Enums.FederationType
        Return PermissionService.FederationCommunityCheck(communityId)
    End Function

    Public Function AccessToCommunity(
                                     ByVal PersonID As Integer, ByVal CommunityID As Integer,
                                     ByVal CommunityPath As String,
                                     ByVal oResourceConfig As ResourceManager,
                                     ByVal LoadUrl As String, ByVal UpdateAccessDate As Boolean) _
                                 As lm.Comol.Core.DomainModel.SubscriptionStatus

        '_Session("Track") = "accessTocommunity_00"
        Dim fed As lm.Comol.Core.BaseModules.Federation.Enums.FederationType = FederationNS.Enums.FederationType.None



        Try
            fed = PermissionService.FederationCommunityCheck(CommunityID)
        Catch ex As Exception
            _Session("Track") = ex.ToString()
        End Try


        If (fed <> lm.Comol.Core.BaseModules.Federation.Enums.FederationType.None) Then

            Dim result As lm.Comol.Core.BaseModules.Federation.Enums.FederationResult = CheckUser(PersonID)

            '    PermissionService.FederationUserCheck(
            '        CommunityID,
            '        PersonID,
            '        SystemSettings.FederationSettings)

            If (result = FederationNS.Enums.FederationResult.CommunityNotFederated) Then
                Return lm.Comol.Core.DomainModel.SubscriptionStatus.notFederated
            End If
            '_Session("Track") = "accessTocommunity_00"
        End If

        Dim iResponse As lm.Comol.Core.DomainModel.SubscriptionStatus = lm.Comol.Core.DomainModel.SubscriptionStatus.none
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim RoleID As Integer = Main.TipoRuoloStandard.AccessoNonAutenticato

        Try
            oTreeComunita.Directory = ProfilePath & PersonID & "\"
            oTreeComunita.Nome = PersonID & ".xml"
        Catch ex As Exception

        End Try

        Try
            Dim oComunita As New COL_Comunita
            oComunita.Id = CommunityID
            oComunita.Estrai()
            If oComunita.Errore = Errori_Db.None Then
                If oComunita.isBloccata Then
                    oTreeComunita.CambiaIsBloccata(CommunityID, True)
                    iResponse = lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
                Else
                    Dim oIscrizione As New COL_RuoloPersonaComunita
                    oIscrizione.Estrai(CommunityID, PersonID)
                    If oIscrizione.Errore = Errori_Db.None Then
                        If oIscrizione.Abilitato AndAlso oIscrizione.Attivato And (oIscrizione.TipoRuolo.Id = Main.TipoRuoloStandard.AccessoNonAutenticato Or oIscrizione.TipoRuolo.Id > 0) Then
                            _Session("IdComunita") = CommunityID
                            If _Session("LogonAs") = False AndAlso UpdateAccessDate Then
                                oIscrizione.UpdateUltimocollegamento()
                            End If
                            RoleID = oIscrizione.TipoRuolo.Id
                            _Session("IdRuolo") = RoleID
                            _Session("RLPC_ID") = oIscrizione.Id
                            '_Session("Track") = "accessTocommunity_01"
                            Dim oListaServizi As GenericCollection(Of PlainServizioComunita)
                            oListaServizi = PlainServizioComunita.ElencaByComunita(RoleID, CommunityID)

                            If oListaServizi.Count > 0 Then
                                Dim ArrPermessi(oListaServizi.Count - 1, 2) As String
                                Dim indice As Integer = 0
                                For Each oServizio As PlainServizioComunita In oListaServizi
                                    ArrPermessi(indice, 0) = oServizio.Codice
                                    ArrPermessi(indice, 1) = oServizio.ID
                                    ArrPermessi(indice, 2) = oServizio.Permessi
                                    indice += 1
                                Next
                                _Session("ArrPermessi") = ArrPermessi
                            Else
                                _Session("ArrPermessi") = Nothing
                            End If
                            _Session("ORGN_id") = oComunita.Organizzazione.Id

                            Dim totale As Integer
                            Dim ArrComunita(,) As String = Nothing

                            With oComunita
                                Dim Path As String = ""
                                Dim ColumIndex As Integer = 0
                                Dim FathersID() As String
                                FathersID = CommunityPath.Split(".")

                                'If Me.isPortalCommunity Then
                                For Each oID As String In FathersID
                                    If IsNumeric(oID) AndAlso oID > 0 Then
                                        ReDim Preserve ArrComunita(3, ColumIndex)
                                        ArrComunita(0, ColumIndex) = oID
                                        ArrComunita(1, ColumIndex) = COL_Comunita.EstraiNomeBylingua(oID, _Session("LinguaID"))
                                        If Path = "" Then
                                            Path = "." & oID & "."
                                        Else
                                            Path = Path & oID & "."
                                        End If
                                        ArrComunita(2, ColumIndex) = Path
                                        ArrComunita(3, ColumIndex) = oPersona.GetIDRuoloForComunita(oID)
                                        ColumIndex += 1
                                    End If
                                Next
                                _Session("ArrComunita") = ArrComunita
                                _Session("limbo") = False
                                'Else 'altrimento lo faccio per passi successivi
                                '    Try
                                '        If TypeOf (_Session("ArrComunita")) Is Array(of String)) then
                                '        End If

                                '        ArrComunita = _Session("ArrComunita")
                                '        totale = UBound(ArrComunita, 2) 'recupero il numero di comunità dell'array
                                '        Dim Last_Path As String = ""
                                '        Path = ArrComunita(2, totale)
                                '        Last_Path = Right(CommunityPath, CommunityPath.Length - Path.Length)
                                '        For Each oID As String In FathersID
                                '            If IsNumeric(oID) AndAlso oID > 0 Then
                                '                ReDim Preserve ArrComunita(3, ColumIndex)
                                '                ArrComunita(0, ColumIndex) = oID
                                '                ArrComunita(1, ColumIndex) = COL_Comunita.EstraiNomeBylingua(oID, _Session("LinguaID"))

                                '                If Path = "" Then
                                '                    Path = "." & oID & "."
                                '                Else
                                '                    Path = Path & oID & "."
                                '                End If
                                '                ArrComunita(2, ColumIndex) = Path
                                '                ArrComunita(3, ColumIndex) = oPersona.GetIDRuoloForComunita(oID)
                                '                ColumIndex += 1
                                '            End If
                                '        Next
                                '        _Session("ArrComunita") = ArrComunita
                                '        _Session("limbo") = False
                                '    Catch ex As Exception

                                '    End Try
                                'End If
                            End With
                            oComunita.RegistraAccesso(CommunityID, PersonID, oResourceConfig.getValue("systemDBcodice"))
                            If UpdateAccessDate Then
                                Me.SendNotificationUpdateCommunityAccess(PersonID, CommunityID, oIscrizione.UltimoCollegamento)
                            End If
                            oTreeComunita.Update(oComunita, CommunityPath, oComunita.GetNomeResponsabile_NomeCreatore, oIscrizione)

                            _Session("AdminForChange") = False
                            _Session("CMNT_path_forAdmin") = ""
                            _Session("idComunita_forAdmin") = ""

                            ' REGISTRAZIONE EVENTO
                            _Session("TPCM_ID") = oComunita.TipoComunita.ID


                            ApiTokenSetCommunity(oComunita.Id)

                            If Not String.IsNullOrEmpty(LoadUrl) Then
                                Me.RedirectToUrl(LoadUrl)
                            ElseIf oComunita.ShowCover(PersonID) Then
                                If oIscrizione.SaltaCopertina Then
                                    Me.RedirectToUrl(RedirectToDefaultPage(CommunityID, PersonID))
                                Else
                                    Me.RedirectToUrl("Generici/Cover.aspx")
                                End If
                            Else
                                Me.RedirectToUrl(RedirectToDefaultPage(CommunityID, PersonID)) ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
                            End If
                            iResponse = lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
                        ElseIf oIscrizione.Attivato = False Then
                            oTreeComunita.CambiaAttivazione(CommunityID, False, Nothing)
                            iResponse = lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
                        ElseIf oIscrizione.Abilitato = False Then
                            oTreeComunita.CambiaAbilitazione(CommunityID, False)
                            iResponse = lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
                        End If
                    End If
                End If
            Else
                _Session("Track") = "accessTocommunity_dB_Err"
                oTreeComunita.Delete(CommunityID, CommunityPath)
            End If
        Catch ex As Exception
            _Session("Track") = ex.ToString()
            Dim err As String = ex.ToString()
        End Try
        Return iResponse
    End Function
    Public Function GetCommunityDefaultPage(ByVal idCommunity As Integer, ByVal idPerson As Integer) As String
        Return RedirectToDefaultPage(idCommunity, idPerson)
    End Function

    Private Function RedirectToDefaultPage(ByVal CommunityID As Integer, ByVal PersonID As Integer) As String
        Dim urlDefault As String = "", Codice As String = ""
        Dim DefaultPageID As Integer
        Dim hasDefaultPage As Boolean = False
        Dim urlRedirect As String = ""

        hasDefaultPage = COL_Comunita.GetDefaultPage(CommunityID, urlDefault, Codice, DefaultPageID)
        If Not hasDefaultPage Or urlDefault = "" Then
            urlRedirect = "Comunita/comunita.aspx"
        Else
            Dim Redirigi As Boolean = False
            Redirigi = True 'CanRedirectToDefaultPage(Codice, CommunityID, PersonID)
            If Redirigi Then
                urlRedirect = urlDefault
            Else
                urlRedirect = "Comunita/comunita.aspx"
            End If
        End If

        Return urlRedirect
    End Function

#End Region


#Region "Portal Access"
    Public Sub LogonUser(user As lm.Comol.Core.DomainModel.Person, idProvider As Long, providerUrl As String, idUserDefaultIdOrganization As Int32)
        LogonUser(user, 0, idProvider, providerUrl, idUserDefaultIdOrganization)
    End Sub
    Public Sub LogonUser(
                        user As lm.Comol.Core.DomainModel.Person,
                        idDefaultCommunity As Integer,
                        idProvider As Long,
                        providerUrl As String,
                        idUserDefaultIdOrganization As Int32)

        ''Federation:
        CheckUser(user.Id, True)



        Me.isPortalCommunity = True
        Me.isModalitaAmministrazione = False
        Me.AmministrazioneComunitaID = 0

        _Session("IdRuolo") = ""
        _Session("IdComunita") = 0
        _Session("ArrPermessi") = ""
        _Session("ArrComunita") = ""
        _Session("RLPC_ID") = ""
        _Session("CMNT_path_forAdmin") = ""

        Dim oPersona As New COL_Persona With {.ID = user.Id}
        oPersona.EstraiTutto(user.LanguageID)

        If idUserDefaultIdOrganization = 0 Then
            idUserDefaultIdOrganization = oPersona.GetOrganizzazioneDefault
        End If
        UserDefaultIdOrganization = idUserDefaultIdOrganization
        _Session("objPersona") = oPersona
        _Session("ORGN_id") = idUserDefaultIdOrganization
        _Session("Istituzione") = oPersona.GetIstituzione

        Dim LangCode As String = "it-IT"
        Dim LangId As Integer = 0

        Try
            Dim oLingua As New Lingua
            If Me.NewLinguaID > 0 AndAlso Me.NewLinguaID <> user.LanguageID Then
                oLingua.ID = Me.NewLinguaID
                oPersona.SalvaImpostazioneLingua(user.LanguageID)
                oPersona.Estrai(user.LanguageID)
            Else
                oLingua = ManagerLingua.GetByID(user.LanguageID)
            End If
            Me.NewLinguaID = 0
            _Session("LinguaID") = oLingua.ID
            _Session("LinguaCode") = oLingua.Codice
            LangCode = oLingua.Codice
            LangId = oLingua.ID
            _Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Icon = oLingua.Icona, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        Catch ex As Exception
            _Session("LinguaID") = 1
            _Session("LinguaCode") = "it-IT"
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        End Try

        'Aggiornamento file XML
        Dim userProfilePath As String = BaseUrlDrivePath & "profili\" & user.Id.ToString & "\"

        Dim oTreeComunita As New COL_TreeComunita
        Try
            oTreeComunita.Directory = userProfilePath
            oTreeComunita.Nome = user.Id.ToString & ".xml"
            oTreeComunita.AggiornaInfo(user.Id.ToString, LinguaID, SystemSettings.CodiceDB, True)
        Catch ex As Exception

        End Try

        'Memorizzo impostazioni utente
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Try
            oImpostazioni.Directory = userProfilePath
            oImpostazioni.Nome = "settings_" & user.Id.ToString & ".xml"
            If oImpostazioni.Exist Then
                oImpostazioni.RecuperaImpostazioni()
                _Session("oImpostazioni") = oImpostazioni
            Else
                _Session("oImpostazioni") = Nothing
            End If
        Catch ex As Exception
            _Session("oImpostazioni") = Nothing
        End Try


        Try
            oPersona.RegistraAccesso(Me.SystemSettings.CodiceDB)
        Catch ex As Exception

        End Try

        CurrentModule = Nothing
        AddLoginAction()

        Dim LinkElenco As String = Me.SystemSettings.Presenter.DefaultLogonPage
        If LinkElenco = "" Then
            Try
                If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
                    LinkElenco = "Comunita/EntrataComunita.aspx"
                Else
                    LinkElenco = "Comunita/NavigazioneTreeView.aspx"
                End If
            Catch ex As Exception
                LinkElenco = "Comunita/EntrataComunita.aspx"
            End Try
        End If



        WriteLoginCookie(user, idProvider, providerUrl)

        ApiTokenRefresh(user.Id, 0, LangId, LangCode, True)

        Dim dto As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = ReadLogoutAccessCookie()
        Dim oResult As dtoLogoutAccess = Nothing
        Dim display As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
        Dim idCommunity As Integer = 0
        Dim url As String = ""
        If IsNothing(dto) Then
            oResult = Me.ReadLogoutAccessCookie(user.Id, user.Login)
            If Not IsNothing(oResult) Then
                If oResult.PageUrl <> "" AndAlso oResult.isDownloading Then
                    url = oResult.PageUrl
                End If

                idCommunity = oResult.CommunityID
            End If
        ElseIf Not String.IsNullOrEmpty(dto.DestinationUrl) Then
            If dto.IdPerson = 0 OrElse dto.IdPerson = user.Id Then : url = dto.DestinationUrl

            End If
            idCommunity = dto.IdCommunity
            display = dto.Display
        End If
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(LinguaCode)

        Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus = lm.Comol.Core.DomainModel.SubscriptionStatus.none
        If Not String.IsNullOrEmpty(url) Then
            Select Case display
                Case lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
                    If Not IsNothing(oResult) Then
                        If oResult.isDownloading Then
                            RedirectToUrl(LinkElenco)
                        ElseIf idCommunity > 0 Then
                            ClearLogoutAccessCookie()
                            AccessToCommunity(oPersona.ID, idCommunity, oResourceConfig, oResult.PageUrl, True)
                        End If
                    ElseIf String.IsNullOrWhiteSpace(url) Then
                        ClearLogoutAccessCookie()
                        If idDefaultCommunity > 0 AndAlso idCommunity <= 0 Then
                            status = AccessToCommunity(oPersona.ID, idDefaultCommunity, oResourceConfig, True)
                        ElseIf idDefaultCommunity > 0 AndAlso idCommunity > 0 Then
                            status = AccessToCommunity(oPersona.ID, idCommunity, oResourceConfig, True)
                        Else
                            RedirectToUrl(LinkElenco)
                        End If
                    Else
                        RedirectToUrl(LinkElenco)
                        'AccessToCommunity(oPersona.ID, idCommunity, oResourceConfig, url, True)
                    End If
                Case lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
                    Me.ClearLogoutAccessCookie()
                    If idCommunity = 0 Then
                        RedirectToUrl(url)
                    ElseIf idCommunity > 0 Then
                        status = AccessToCommunity(user.Id, idCommunity, oResourceConfig, url, True)
                        'RedirectToUrl(url)
                        If status = lm.Comol.Core.DomainModel.SubscriptionStatus.none Then
                            Me.RedirectToUrl(LinkElenco)
                        End If
                    End If
                Case Else
                    If idDefaultCommunity > 0 Then
                        status = AccessToCommunity(user.Id, idDefaultCommunity, oResourceConfig, True)
                        If status = lm.Comol.Core.DomainModel.SubscriptionStatus.none Then
                            Me.RedirectToUrl(LinkElenco)
                        End If
                    End If
            End Select
        Else
            Me.ClearLogoutAccessCookie()
            If idDefaultCommunity > 0 Then
                status = AccessToCommunity(user.Id, idDefaultCommunity, oResourceConfig, True)
                If status = lm.Comol.Core.DomainModel.SubscriptionStatus.none Then
                    Me.RedirectToUrl(LinkElenco)
                End If
            Else
                Me.RedirectToUrl(LinkElenco)
            End If
        End If
    End Sub

    Public Sub LogonUserIntoSystem(user As lm.Comol.Core.DomainModel.Person, updateXML As Boolean, idProvider As Long, providerUrl As String)
        Me.isPortalCommunity = True
        Me.isModalitaAmministrazione = False
        Me.AmministrazioneComunitaID = 0

        _Session("IdRuolo") = ""
        _Session("IdComunita") = 0
        _Session("ArrPermessi") = ""
        _Session("ArrComunita") = ""
        _Session("RLPC_ID") = ""
        _Session("CMNT_path_forAdmin") = ""

        Dim oPersona As New COL_Persona With {.ID = user.Id}
        oPersona.Estrai(user.LanguageID)

        _Session("objPersona") = oPersona
        _Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
        _Session("Istituzione") = oPersona.GetIstituzione

        Dim LangId As Integer = 0
        Dim LangCode As String = "it-IT"

        Try
            Dim oLingua As New Lingua
            If Me.NewLinguaID > 0 AndAlso Me.NewLinguaID <> user.LanguageID Then
                oLingua.ID = Me.NewLinguaID
                oPersona.SalvaImpostazioneLingua(user.LanguageID)
                oPersona.Estrai(user.LanguageID)
            Else
                oLingua = ManagerLingua.GetByID(user.LanguageID)
            End If
            Me.NewLinguaID = 0
            _Session("LinguaID") = oLingua.ID
            LangId = oLingua.ID

            _Session("LinguaCode") = oLingua.Codice
            LangCode = oLingua.Codice

            _Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Icon = oLingua.Icona, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        Catch ex As Exception
            _Session("LinguaID") = 1
            _Session("LinguaCode") = "it-IT"
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        End Try

        Dim userProfilePath As String = BaseUrlDrivePath & "profili\" & user.Id.ToString & "\"
        If updateXML Then
            'Aggiornamento file XML

            Dim oTreeComunita As New COL_TreeComunita
            Try
                oTreeComunita.Directory = userProfilePath
                oTreeComunita.Nome = user.Id.ToString & ".xml"
                oTreeComunita.AggiornaInfo(user.Id.ToString, LinguaID, SystemSettings.CodiceDB, True)
            Catch ex As Exception

            End Try
        End If

        'Memorizzo impostazioni utente
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Try
            oImpostazioni.Directory = userProfilePath
            oImpostazioni.Nome = "settings_" & user.Id.ToString & ".xml"
            If oImpostazioni.Exist Then
                oImpostazioni.RecuperaImpostazioni()
                _Session("oImpostazioni") = oImpostazioni
            Else
                _Session("oImpostazioni") = Nothing
            End If
        Catch ex As Exception
            _Session("oImpostazioni") = Nothing
        End Try


        Try
            oPersona.RegistraAccesso(Me.SystemSettings.CodiceDB)
        Catch ex As Exception

        End Try

        CurrentModule = Nothing
        AddLoginAction()

        WriteLoginCookie(user, idProvider, providerUrl)

        ApiTokenRefresh(user.Id, 0, LangId, LangCode, True)
    End Sub

    Public Sub LogonAsUser(user As COL_Persona)
        Me.isPortalCommunity = True
        Me.isModalitaAmministrazione = False
        Me.AmministrazioneComunitaID = 0

        _Session("IdRuolo") = ""
        _Session("IdComunita") = 0
        _Session("ArrPermessi") = ""
        _Session("ArrComunita") = ""
        _Session("RLPC_ID") = ""
        _Session("CMNT_path_forAdmin") = ""

        'Dim oPersona As New COL_Persona With {.ID = user.Id}
        'oPersona.Estrai(user.Lingua.ID)

        _Session("objPersona") = user
        _Session("ORGN_id") = user.GetOrganizzazioneDefault
        _Session("Istituzione") = user.GetIstituzione

        Dim LangId As Integer = 0
        Dim LangCode As String = "it-IT"

        Try
            Dim oLingua As New Lingua
            oLingua = user.Lingua
            oLingua = ManagerLingua.GetByID(user.Lingua.ID)

            Me.NewLinguaID = 0
            _Session("LinguaID") = oLingua.ID
            LangId = oLingua.ID

            _Session("LinguaCode") = oLingua.Codice
            LangCode = oLingua.Codice

            _Session("UserLanguage") = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Icon = oLingua.Icona, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        Catch ex As Exception
            _Session("LinguaID") = 1
            _Session("LinguaCode") = "it-IT"
            Me.SetCookies(_Session("LinguaID"), _Session("LinguaCode"))
        End Try

        'Aggiornamento file XML
        Dim userProfilePath As String = BaseUrlDrivePath & "profili\" & user.ID.ToString & "\"

        Dim oTreeComunita As New COL_TreeComunita
        Try
            oTreeComunita.Directory = userProfilePath
            oTreeComunita.Nome = user.ID.ToString & ".xml"
            oTreeComunita.AggiornaInfo(user.ID.ToString, LinguaID, SystemSettings.CodiceDB, True)
        Catch ex As Exception

        End Try

        'Memorizzo impostazioni utente
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Try
            oImpostazioni.Directory = userProfilePath
            oImpostazioni.Nome = "settings_" & user.ID.ToString & ".xml"
            If oImpostazioni.Exist Then
                oImpostazioni.RecuperaImpostazioni()
                _Session("oImpostazioni") = oImpostazioni
            Else
                _Session("oImpostazioni") = Nothing
            End If
        Catch ex As Exception
            _Session("oImpostazioni") = Nothing
        End Try


        ApiTokenRefresh(user.ID, 0, LangId, LangCode, True)


        CurrentModule = Nothing

        Dim LinkElenco As String = Me.SystemSettings.Presenter.DefaultLogonPage
        If LinkElenco = "" Then
            Try
                If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
                    LinkElenco = "Comunita/EntrataComunita.aspx"
                Else
                    LinkElenco = "Comunita/NavigazioneTreeView.aspx"
                End If
            Catch ex As Exception
                LinkElenco = "Comunita/EntrataComunita.aspx"
            End Try
        End If
        Me.RedirectToUrl(LinkElenco)
    End Sub
#End Region

    Public ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
        Get
            If IsNumeric(_Request.QueryString("CV")) Then
                Try
                    Return _Request.QueryString("CV")
                Catch ex As Exception
                    Return lm.Comol.Core.DomainModel.ContentView.viewAll
                End Try
            Else
                Return lm.Comol.Core.DomainModel.ContentView.viewAll
            End If
        End Get
    End Property

#Region "Logo Istituzione"
    Private _NewSkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property NewSkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_NewSkinService) Then
                _NewSkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _NewSkinService
        End Get
    End Property

    ''' <summary>
    ''' Path fisico completo relativo all'immagine caricata per il logo dell'header.
    ''' </summary>
    ''' <value></value>
    ''' <returns>Es: E:\Inetpub\ProgettiWeb\LM_ComolElle3\Src\Comunita_OnLine\File\Skins\6\15_Comol.png</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LogoIstituzionePath As String
        Get
            Dim path As String = ""

            Dim Organization_Id = GetSkinIdOrganization()
            Dim Community_Id = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

            Dim DefaultLogoPath As String = _Server.MapPath(Me.BaseUrl & Me.SystemSettings.SkinSettings.HeadLogo.Url)
            'Server.MapPath(Me.BaseUrl & Me.SystemSettings.SkinSettings.HeadLogo.Url)

            path = NewSkinService.GetlogoIstituzioneFullPath(
                Community_Id,
                Organization_Id,
                Me.SystemSettings.SkinSettings.SkinPhisicalPath,
                Me.CurrentContext.UserContext.Language.Code,
                Me.SystemSettings.DefaultLanguage.Codice, DefaultLogoPath)

            'If path = "" Then
            '    path = _Server.MapPath(Me.ApplicationUrlBase & SystemSettings.Presenter.LogoIstituzione.Replace("/", "\").Remove(0, 1))
            'End If

            Return path
        End Get
    End Property

    ''' <summary>
    ''' Url dell'immagine caricata per il logo dell'header.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Controllare!!!</remarks>
    Public ReadOnly Property LogoIstituzioneUrl As String
        Get
            Dim path As String = ""

            Dim Organization_Id = GetSkinIdOrganization()
            Dim Community_Id = Me.CurrentContext.UserContext.CurrentCommunityID 'PageUtility.ComunitaCorrente.Id

            path = NewSkinService.GetlogoIstituzioneUrl(
                Community_Id,
                Organization_Id,
                "",
                Me.CurrentContext.UserContext.Language.Code,
                Me.SystemSettings.DefaultLanguage.Codice, Me.BaseUrl & Me.SystemSettings.SkinSettings.HeadLogo.Url)

            'If path = "" Then
            '    path = SystemSettings.Presenter.LogoIstituzione '.Replace("\", "/").Remove(0, 1)
            'End If

            Return path
        End Get
    End Property

#End Region


#Region "Token notification"
    Public Sub NotifyTokenError(idPerson As Integer, ByVal urlToken As lm.Comol.Core.Authentication.dtoUrlToken, status As lm.Comol.Core.Authentication.UrlProviderResult)
        Dim settings As NotificationErrorSettings = SystemSettings.NotificationErrorService
        If settings.isSendingEnabled(ErrorsNotificationService.ErrorType.GenericWebError) Then
            Dim notificationService As ErrorsNotificationService.iErrorsNotificationService = Nothing
            Try
                notificationService = New ErrorsNotificationService.iErrorsNotificationServiceClient()
                Dim oError As ErrorsNotificationService.GenericWebError = New ErrorsNotificationService.GenericWebError()
                With oError
                    'CONTEXT VARIABLES
                    .ServerName = _Server.MachineName
                    .SentDate = Now
                    .Day = .SentDate.Date
                    .ComolUniqueID = settings.ComolUniqueID

                    Try
                        .CommunityID = 0
                        .UserID = idPerson
                        .ModuleID = 0
                        .ModuleCode = "UrlAuthentication"
                    Catch ex As Exception

                    End Try
                    .QueryString = _Request.QueryString.ToString
                    .UniqueID = System.Guid.NewGuid()
                    .Url = _Request.Url.AbsolutePath
                    .Persist = settings.FindPersistTo(ErrorsNotificationService.ErrorType.GenericWebError)
                    .Type = ErrorsNotificationService.ErrorType.GenericWebError

                    .Message = "Identifier = " & urlToken.Identifier & vbCrLf & "Value=" & urlToken.Value & vbCrLf & "DecriptedValue=" & urlToken.DecriptedValue & vbCrLf & "FullDecriptedValue=" & urlToken.Evaluation.FullDecriptedValue & vbCrLf & "ExpiredMessage=" & urlToken.Evaluation.ExpiredMessage & vbCrLf & "ExceptionString=" & urlToken.Evaluation.ExceptionString & vbCrLf & "TokenException=" & urlToken.Evaluation.TokenException & vbCrLf & "Status =" & status.ToString
                End With
                notificationService.sendGenericWebError(oError)
                If Not IsNothing(notificationService) Then
                    Dim service As System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService) = DirectCast(notificationService, System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService))
                    service.Abort()
                    service = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(notificationService) Then
                    Dim service As System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService) = DirectCast(notificationService, System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService))
                    service.Abort()
                    service = Nothing
                End If
            End Try
        End If

    End Sub

#End Region



#Region "Federation"

    Private Const FederationAppKey As String = "FederatedUsers"
    Public MaxLifeTime As TimeSpan = New TimeSpan(1, 0, 0)

    Public Function CheckUser(UserId As Integer, Optional ByVal update As Boolean = False) As FederationNS.Enums.FederationResult



        Dim result As FederationNS.Enums.FederationResult = FederationNS.Enums.FederationResult.Unknow

        Try

            Dim FederationData As FederationNS.Domain.dtoUserfederationData

            'If Not IsNothing(_Application.Item(FederationAppKey)) Then

            'End If

            Dim Federated As Dictionary(Of Integer, FederationNS.Domain.dtoUserfederationData)

            Try
                Federated = _Application.Item(FederationAppKey)
            Catch ex As Exception

            End Try

            If IsNothing(Federated) Then
                Federated = New Dictionary(Of Integer, FederationNS.Domain.dtoUserfederationData)
            End If

            If (Federated.ContainsKey(UserId)) Then
                FederationData = Federated(UserId)
            End If

            If IsNothing(FederationData) OrElse FederationData.LifeTime >= MaxLifeTime OrElse update Then
                FederationData = New FederationNS.Domain.dtoUserfederationData()
                FederationData.UserId = UserId
                'FederationData.Creation = DateTime.Now()
                FederationData.CommunityId = 0
                FederationData.Result = PermissionService.FederationUserCheck(0, UserId, SystemSettings.FederationSettings)
            End If



            FederationData.Creation = DateTime.Now()
            result = FederationData.Result

            Federated(UserId) = FederationData


            _Application.Item(FederationAppKey) = Federated




            Return result
        Catch ex As Exception

        End Try


    End Function



#End Region


#Region "Trap"
    ''' <summary>
    ''' Test invio trap login
    ''' </summary>
    Public Sub SendTrapLoginTest()
        SendTrapLogin()
    End Sub

    ''' <summary>
    ''' Invio del trap dopo la login
    ''' </summary>
    Private Sub SendTrapLogin()

        If AllowSendTrapActions AndAlso Not IsNothing(TrapSender) Then
            Dim actionvalue As WsSnmtp.dtoActionValues = New WsSnmtp.dtoActionValues()
            With actionvalue
                .Progressive = TrapProgressive
                .EventId = TrapIdEnums.LoginSuccess
                .User = New WsSnmtp.dtoUserValues()
                .Action = New WsSnmtp.dtoActionData()
            End With

            With actionvalue.User
                .id = Me.CurrentUser.ID
                .login = Me.CurrentUser.Login
                .mail = Me.CurrentUser.Mail
                .name = Me.CurrentUser.Nome
                .surname = Me.CurrentUser.Cognome
                .taxCode = Me.CurrentUser.CodFiscale
                .Ip = Me.ClientIPadress
                .ProxyIp = Me.ProxyIPadress
            End With

            With actionvalue.Action
                .ActionCodeId = "4624"
                .ActionTypeId = "login"
                .CommunityId = Me.WorkingCommunityID
                .CommunityIsFederated = False
                .InteractionType = InteractionType.Generic
                .ModuleCode = "Login"
                .ModuleId = CurrentModule.ID
                .SuccessInfo = "Success"
                .ObjectId = 0
                .ObjectType = 0
            End With

            Dim TrapId As Integer = TrapIdEnums.LoginSuccess
            TrapSender.SendTrapActionValue(TrapId, actionvalue)
        End If

    End Sub

    ''' <summary>
    ''' Invio del trap al logout
    ''' </summary>
    Private Sub SendTrapLogout()
        If AllowSendTrapActions AndAlso Not IsNothing(TrapSender) Then
            Dim actionvalue As WsSnmtp.dtoActionValues = New WsSnmtp.dtoActionValues()
            With actionvalue
                .Progressive = TrapProgressive
                .EventId = TrapIdEnums.LoginSuccess
                .User = New WsSnmtp.dtoUserValues()
                .Action = New WsSnmtp.dtoActionData()
            End With

            With actionvalue.User
                .id = Me.CurrentUser.ID
                .login = Me.CurrentUser.Login
                .mail = Me.CurrentUser.Mail
                .name = Me.CurrentUser.Nome
                .surname = Me.CurrentUser.Cognome
                .taxCode = Me.CurrentUser.CodFiscale
                .Ip = Me.ClientIPadress
                .ProxyIp = Me.ProxyIPadress
            End With

            With actionvalue.Action
                .ActionCodeId = "4634"
                .ActionTypeId = "login"
                .CommunityId = Me.WorkingCommunityID
                .CommunityIsFederated = False
                .InteractionType = InteractionType.Generic
                .ModuleCode = "Logout"
                .ModuleId = CurrentModule.ID
                .ObjectId = 0
                .ObjectType = 0
                .SuccessInfo = "Success"
            End With

            Dim TrapId As Integer = TrapIdEnums.LogOut
            TrapSender.SendTrapActionValue(TrapId, actionvalue)
        End If
    End Sub

    ''' <summary>
    ''' Trap login fallita
    ''' </summary>
    ''' <param name="login">Login inserita dall'utente</param>
    ''' <param name="info">Informazioni aggiuntive</param>
    Public Sub SendTrapLoginFailed(ByVal login As String, ByVal info As String)
        If AllowSendTrapActions AndAlso Not IsNothing(TrapSender) Then
            Dim actionvalue As WsSnmtp.dtoActionValues = New WsSnmtp.dtoActionValues()
            With actionvalue
                .Progressive = TrapProgressive
                .EventId = TrapIdEnums.LoginSuccess
                .User = New WsSnmtp.dtoUserValues()
                .Action = New WsSnmtp.dtoActionData()
            End With

            With actionvalue.User
                .id = Me.CurrentUser.ID
                .login = login
                .mail = Me.CurrentUser.Mail
                .name = Me.CurrentUser.Nome
                .surname = Me.CurrentUser.Cognome
                .taxCode = Me.CurrentUser.CodFiscale
                .Ip = Me.ClientIPadress
                .ProxyIp = Me.ProxyIPadress
            End With

            If (String.IsNullOrWhiteSpace(info)) Then
                info = login
            Else
                info = String.Format("{0},{1}", login, info)
            End If

            With actionvalue.Action
                .ActionCodeId = "4634"
                .ActionTypeId = "login"
                .CommunityId = Me.WorkingCommunityID
                .CommunityIsFederated = False
                .InteractionType = InteractionType.Generic
                .ModuleCode = "Login"
                If (String.IsNullOrEmpty(info)) Then
                    .SuccessInfo = String.Format("LoginFailed")
                Else
                    .SuccessInfo = String.Format("LoginFailed({0})", info)
                End If

                .ModuleId = CurrentModule.ID
                .ObjectId = 0
                .ObjectType = 0
            End With

            Dim TrapId As Integer = TrapIdEnums.LoginFailed
            TrapSender.SendTrapActionValue(TrapId, actionvalue)
        End If
    End Sub

    ''' <summary>
    ''' Recuepra un oggetto dtoActionValue con i parametri preimpostati
    ''' </summary>
    ''' <returns></returns>
    Public Function TrapGetCurrentValues() As WsSnmtp.dtoActionValues
        Dim actionvalue As WsSnmtp.dtoActionValues = New WsSnmtp.dtoActionValues()
        With actionvalue
            .Progressive = TrapProgressive
            .EventId = TrapIdEnums.LoginSuccess
            .User = New WsSnmtp.dtoUserValues()
            .Action = New WsSnmtp.dtoActionData()
        End With

        With actionvalue.User
            .id = Me.CurrentUser.ID
            .login = Me.CurrentUser.Login
            .mail = Me.CurrentUser.Mail
            .name = Me.CurrentUser.Nome
            .surname = Me.CurrentUser.Cognome
            .taxCode = Me.CurrentUser.CodFiscale
            .Ip = Me.ClientIPadress
            .ProxyIp = Me.ProxyIPadress
        End With

        With actionvalue.Action
            '.ActionCodeId = "4634"
            '.ActionTypeId = "login"
            .CommunityId = Me.WorkingCommunityID
            .CommunityIsFederated = False
            '.InteractionType = InteractionType.Generic
            '.ModuleCode = "Login"
            'If (String.IsNullOrEmpty(info)) Then
            '    .SuccessInfo = String.Format("LoginFailed")
            'Else
            '    .SuccessInfo = String.Format("LoginFailed({0})", info)
            'End If

            .ModuleId = CurrentModule.ID
            '.ObjectId = 0
            '.ObjectType = 0
        End With

        Return actionvalue
    End Function

    ''' <summary>
    ''' Invio trap generico
    ''' </summary>
    ''' <param name="TrapId">Id trap</param>
    ''' <param name="actionvalue">Action value</param>
    Public Sub TrapSendGeneric(ByVal TrapId As Integer, ByVal actionvalue As WsSnmtp.dtoActionValues)
        If AllowSendTrapActions AndAlso Not IsNothing(TrapSender) Then

            If actionvalue.Progressive = 0 Then
                actionvalue.Progressive = TrapProgressive
            End If

            TrapSender.SendTrapActionValue(TrapId, actionvalue)
        End If
    End Sub

    ''' <summary>
    ''' Invio trap generico, impostando i singoli parametri
    ''' </summary>
    ''' <param name="TrapId">Id trap</param>
    ''' <param name="ActionCodeId">Codice azione</param>
    ''' <param name="ActionTypeId">Tipo azione</param>
    ''' <param name="InteractionType">Tipo interazione</param>
    ''' <param name="ModuleCode">Codice modulo</param>
    ''' <param name="Info">Informaizoni da inviare</param>
    ''' <param name="ModuleId">Id modulo</param>
    ''' <param name="ObjectId">Id oggetto</param>
    ''' <param name="ObjectType">Tipo di oggetto</param>
    Public Sub TrapSendGeneric(TrapId As Integer,
                               ActionCodeId As String,
                               ActionTypeId As String,
                               InteractionType As InteractionType,
                               ModuleCode As String,
                               Info As String,
                               ModuleId As String,
                               ObjectId As String,
                               ObjectType As String)


        Dim data As WsSnmtp.dtoActionData = New WsSnmtp.dtoActionData()

        With data
            .ActionCodeId = ActionCodeId
            .ActionTypeId = ActionTypeId
            .InteractionType = InteractionType
            .ModuleCode = ModuleCode
            .SuccessInfo = Info
            .ModuleId = ModuleId
            .ObjectId = ObjectId
            .ObjectType = ObjectType
        End With

        TrapSendGeneric(TrapId, data)
    End Sub

    ''' <summary>
    ''' Invio trap generico
    ''' </summary>
    ''' <param name="TrapId">Id trap</param>
    ''' <param name="data">dtoActionData con i dati da inviare</param>
    Public Sub TrapSendGeneric(TrapId As Integer, data As WsSnmtp.dtoActionData)
        Dim actionvalue As WsSnmtp.dtoActionValues = TrapGetCurrentValues()
        actionvalue.Action = data

        TrapSendGeneric(TrapId, actionvalue)

    End Sub


#End Region
End Class

Public Class dtoObjectToNotify
    Public ObjectTypeID As Integer
    Public ObjectID As String
    Public FullyQualiFiedNameField As String
End Class

''' <summary>
''' ENUM con i moduli notificati via TRAP.
''' </summary>
Public Enum TrapModules
    ''' <summary>
    ''' Nessuno: non in uso
    ''' </summary>
    none = 0
    ''' <summary>
    ''' Login
    ''' </summary>
    Login = -1
    ''' <summary>
    ''' Servizio Bandi
    ''' </summary>
    SRVCFP = 43
End Enum

Module CookieHelper

    Public Const CookieKeyToked As String = "Token"

    Public Const CookieKeyDeviceId As String = "DeviceId"

    Public Const CookieKeyCommunityId As String = "CommunityId"

    Public Const CookieKeyLinkId As String = "LinkId"

    Public Const CookieKeyLangId As String = "LanguageId"

    Public Const CookieKeyLangCode As String = "LanguageCode"

    Public Const CookieKeyServiceCode As String = "ServiceCode"

    Public Const CookieKeyPersonId As String = "PersonId"
End Module
