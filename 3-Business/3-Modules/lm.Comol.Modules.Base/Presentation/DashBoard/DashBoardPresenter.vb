Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.Presentation
    Public Class DashBoardPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        Private _NoticeBoardPermission As ModuleNoticeBoard
        Private _CommunitiesNoticeBoardPermission As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
        Private ReadOnly Property NoticeBoardPermission(Optional ByVal CommunityID As Integer = 0) As ModuleNoticeBoard
            Get
                If IsNothing(_NoticeBoardPermission) AndAlso CommunityID <= 0 Then
                    _NoticeBoardPermission = Me.View.NoticeBoardPermission
                    Return _NoticeBoardPermission
                ElseIf CommunityID > 0 Then
                    _NoticeBoardPermission = (From o In CommunitiesNoticeBoardPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                    If IsNothing(_NoticeBoardPermission) Then
                        _NoticeBoardPermission = New ModuleNoticeBoard
                    End If
                    Return _NoticeBoardPermission
                Else
                    Return _NoticeBoardPermission
                End If
                Return _NoticeBoardPermission
            End Get
        End Property
        Private ReadOnly Property CommunitiesNoticeBoardPermission() As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
            Get
                If IsNothing(_CommunitiesNoticeBoardPermission) Then
                    _CommunitiesNoticeBoardPermission = Me.View.CommunitiesNoticeBoardPermission
                End If
                Return _CommunitiesNoticeBoardPermission
            End Get
        End Property
#End Region

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerNoticeBoard
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerNoticeBoard)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewDashBoard
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.NotificationManager = New lm.Modules.NotificationSystem.Business.ManagerCommunitynews(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewDashBoard)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.NotificationManager = New lm.Modules.NotificationSystem.Business.ManagerCommunitynews(MyBase.AppContext)
        End Sub

        Private _CommonManager As ManagerCommon
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Private _NotificationManager As lm.Modules.NotificationSystem.Business.ManagerCommunitynews
        Public Overloads Property NotificationManager() As lm.Modules.NotificationSystem.Business.ManagerCommunitynews
            Get
                Return _NotificationManager
            End Get
            Set(ByVal value As lm.Modules.NotificationSystem.Business.ManagerCommunitynews)
                _NotificationManager = value
            End Set
        End Property
#End Region

        Public Sub InitView(ByVal ForPortal As Boolean)
            Dim CommunityID As Integer = Me.View.PreloadedCommunityID
            If ForPortal Then
                CommunityID = 0
            ElseIf CommunityID = -1 Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If
            Me.View.MessageCommunityID = CommunityID
            If Not Me.UserContext.isAnonymous Then
                LoadMessage(CommunityID)
                LoadLastTenCommunities()
            Else
                Me.View.AddActionNoPermission(CommunityID, Me.UserContext.CurrentUserID)
                Me.View.NoPermissionToAccess()
            End If
        End Sub

#Region "Noticeboard"
        Private Sub LoadMessage(ByVal CommunityID As Integer)
            Dim oNoticeboard As NoticeBoard
            oNoticeboard = Me.CurrentManager.GetLastMessage(CommunityID)
            If IsNothing(oNoticeboard) Then
                oNoticeboard = New NoticeBoard
            End If
            Me.View.AddShowMessageAction(oNoticeboard.Id, CommunityID)
            Me.ShowMessage(oNoticeboard, CommunityID)
        End Sub

        Private Sub ShowMessage(ByVal oNoticeboard As NoticeBoard, ByVal CommunityID As Integer)
            If HasPermissionToView(CommunityID) Then
                If CommunityID > 0 Then
                    Me.View.setHeaderTitle("community")
                Else
                    Me.View.setHeaderTitle("")
                End If
                Me.View.CurrentMessageID = oNoticeboard.Id
                Me.View.ViewMessage()

                'Dim HTMLurl As String = Me.View.GetNavigationUrl(tmp, NoticeBoardContext.ViewModeType.NewMessageHTML, tmp.SmallView)
                'Dim AdvancedUrl As String = Me.View.GetNavigationUrl(tmp, NoticeBoardContext.ViewModeType.NewMessageADV, tmp.SmallView)
                'Me.View.SetNewMessageUrl(HTMLurl, AdvancedUrl)
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUserID)
                Me.View.NoPermissionToAccess()
            End If
        End Sub
        Private Function HasPermissionToView(ByVal CommunityID As Integer) As Boolean
            Dim oPermission As ModuleNoticeBoard = Me.NoticeBoardPermission(CommunityID)
            Return oPermission.ViewCurrentMessage OrElse oPermission.ViewOldMessage OrElse oPermission.ServiceAdministration
        End Function
       
        Private Function HasPermissionToEdit(ByVal oCommunity As Community) As Boolean
            Dim oPermission As ModuleNoticeBoard
            If oCommunity Is Nothing Then
                oPermission = Me.NoticeBoardPermission(0)
            Else
                oPermission = Me.NoticeBoardPermission(oCommunity.Id)
            End If

            Return oPermission.EditMessage OrElse oPermission.ServiceAdministration
        End Function
#End Region

#Region "Community"
        Private Sub LoadLastTenCommunities()
            Dim oList As List(Of dtoSubscription)
            oList = (From s In Me.CommonManager.GetLastSubscriptions(Me.UserContext.CurrentUserID, 15, True) Select New dtoSubscription(s)).ToList

            If oList.Count > 0 Then
                Dim oNews = (From subs In oList Join rn In Me.NotificationManager.GetCommunityNewsCount(Me.UserContext.CurrentUserID) On subs.CommunityID Equals rn.Id Where subs.Enabled _
                             Select subs.UpdateNewsInfo(rn)).tolist
                ' Retrieve news number for communities
                ' Dim oCommunityTypes As List(of COL_BusinessLogic_v2.
            End If

            Me.View.LoadLastSubscription(oList)
        End Sub



#End Region
    End Class
End Namespace