Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.Presentation
    Public Class NoticeBoardPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        'Private _Permission As ModuleNoticeBoard
        'Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
        'Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleNoticeBoard
        '    Get
        '        If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
        '            _Permission = Me.View.ModulePermission
        '            Return _Permission
        '        ElseIf CommunityID > 0 Then
        '            _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
        '            If IsNothing(_Permission) Then
        '                _Permission = New ModuleNoticeBoard
        '            End If
        '            Return _Permission
        '        Else
        '            Return _Permission
        '        End If
        '        Return _Permission
        '    End Get
        'End Property
        'Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
        '    Get
        '        If IsNothing(_CommunitiesPermission) Then
        '            _CommunitiesPermission = Me.View.CommunitiesPermission()
        '        End If
        '        Return _CommunitiesPermission
        '    End Get
        'End Property
#End Region

        Private _CommonManager As ManagerCommon
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_Bacheca.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        'Private _ContexFromView As NoticeBoardContext
        'Private ReadOnly Property ContexFromView() As NoticeBoardContext
        '    Get
        '        If IsNothing(_ContexFromView) Then
        '            Dim oContext As NoticeBoardContext = Me.View.CurrentNoticeBoardContext
        '            Dim PersonID As Integer = oContext.UserID
        '            If PersonID = 0 Then
        '                PersonID = Me.AppContext.UserContext.CurrentUser.Id
        '            End If
        '            oContext.UserID = PersonID
        '            Dim CommunityID As Integer = oContext.CommunityID
        '            If CommunityID <= 0 Then
        '                CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.CommunityID = CommunityID
        '            If oContext.MessageID > 0 AndAlso oContext.SmallView = NoticeBoardContext.SmallViewType.None Then
        '                oContext.SmallView = NoticeBoardContext.SmallViewType.LastFourMessage
        '                Dim oNoticeboard As NoticeBoard = Me.CurrentManager.GetMessage(oContext.MessageID)
        '                If oNoticeboard.isForPortal Then
        '                    oContext.CommunityID = 0
        '                ElseIf Not oNoticeboard.CommunityOwner Is Nothing Then
        '                    oContext.CommunityID = oNoticeboard.CommunityOwner.Id
        '                End If
        '            End If
        '            Me.View.CurrentNoticeBoardContext = oContext
        '            _ContexFromView = oContext
        '        End If
        '        Return _ContexFromView
        '    End Get
        'End Property

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerNoticeBoard
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerNoticeBoard)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewNoticeBoard
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewNoticeBoard)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        'Public Sub InitView()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView
        '    Dim MessageID As Long = Me.View.PreLoadedMessageID
        '    Dim CommunityID As Integer = Me.View.PreLoadedCommunityID
        '    If CommunityID = -1 AndAlso MessageID = 0 Then
        '        CommunityID = Me.UserContext.CurrentCommunityID
        '    ElseIf MessageID > 0 Then
        '        Dim oNoticeboard As NoticeBoard = Me.CurrentManager.GetMessage(MessageID)
        '        If Not IsNothing(oNoticeboard) Then
        '            If oNoticeboard.isForPortal Then
        '                CommunityID = 0
        '            ElseIf Not oNoticeboard.CommunityOwner Is Nothing Then
        '                CommunityID = oNoticeboard.CommunityOwner.Id
        '            End If
        '        ElseIf CommunityID = -1 Then
        '            CommunityID = Me.UserContext.CurrentCommunityID
        '        End If
        '    End If
        '    oContext.CommunityID = CommunityID
        '    Me.View.MessageCommunityID = CommunityID
        '    If Not Me.UserContext.isAnonymous Then
        '        ChangeView()
        '    Else
        '        Me.View.AddActionNoPermission(CommunityID, ModuleID, 0)
        '        Me.View.NoPermissionToAccess()
        '    End If
        'End Sub
        'Public Sub LoadMessage(ByVal MessageID As Long)
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView
        '    oContext.MessageID = MessageID
        '    oContext.ViewMode = NoticeBoardContext.ViewModeType.Message

        '    Me.View.CurrentNoticeBoardContext = oContext
        '    Me.ChangeView()

        'End Sub

        'Private Sub ChangeView()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView
        '    If oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage OrElse oContext.ViewMode = NoticeBoardContext.ViewModeType.Message Then
        '        LoadMessage(oContext, True)
        '    ElseIf oContext.ViewMode = NoticeBoardContext.ViewModeType.NewMessageADV Then
        '        Me.View.CurrentMessageID = 0
        '        Me.View.SetBackUrlFromEditor = Me.View.GetNavigationUrl(oContext, Me.View.PreLoadedPreviousView, oContext.SmallView)
        '        Me.View.EditMessage(Nothing, False)
        '        If oContext.CommunityID > 0 Then
        '            Me.View.setHeaderTitle("community")
        '        Else
        '            Me.View.setHeaderTitle("")
        '        End If
        '    ElseIf oContext.ViewMode = NoticeBoardContext.ViewModeType.NewMessageHTML Then
        '        Me.View.CurrentMessageID = 0
        '        Me.View.SetBackUrlFromEditor = Me.View.GetNavigationUrl(oContext, Me.View.PreLoadedPreviousView, oContext.SmallView)
        '        Me.View.EditMessage(Nothing, True)
        '        If oContext.CommunityID > 0 Then
        '            Me.View.setHeaderTitle("community")
        '        Else
        '            Me.View.setHeaderTitle("")
        '        End If
        '    End If
        'End Sub

        'Private Sub LoadMessage(ByVal oContext As NoticeBoardContext, ByVal RegisterAction As Boolean)
        '    Dim oNoticeboard As NoticeBoard
        '    Dim isCurrent As Boolean = False
        '    If oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage OrElse oContext.MessageID = 0 Then
        '        oNoticeboard = LastMessage(oContext.CommunityID)
        '        isCurrent = True
        '    Else
        '        oNoticeboard = DataContext.GetById(Of NoticeBoard)(oContext.MessageID)
        '    End If
        '    Dim CommunityID As Integer = Me.View.MessageCommunityID
        '    If CommunityID > 0 Then
        '        Me.View.setHeaderTitle("community")
        '    Else
        '        Me.View.setHeaderTitle("")
        '    End If
        '    'If IsNothing(oNoticeboard) Then
        '    '    CommunityID = #Me.UserContext.CurrentCommunityID#
        '    'ElseIf oNoticeboard.isForPortal Then
        '    '    CommunityID = 0
        '    'Else
        '    '    If Not IsNothing(oNoticeboard.CommunityOwner) Then
        '    '        CommunityID = oNoticeboard.CommunityOwner.Id
        '    '    Else
        '    '        CommunityID = -1
        '    '    End If
        '    'End If

        '    If Not isCurrent Then
        '        Dim oLastMessage As NoticeBoard = LastMessage(CommunityID)
        '        If IsNothing(oLastMessage) = False Then
        '            If IsNothing(oNoticeboard) OrElse oLastMessage.Id = oNoticeboard.Id Then
        '                isCurrent = True
        '            End If
        '        End If
        '    End If
        '    If IsNothing(oNoticeboard) Then
        '        oNoticeboard = New NoticeBoard
        '    End If
        '    If RegisterAction Then
        '        Me.View.AddShowMessageAction(oNoticeboard.Id, CommunityID, ModuleID)
        '    End If
        '    If oContext.PreviousView = NoticeBoardContext.ViewModeType.DashBoard Then
        '        Me.View.SetPreviousURL = Me.View.GetMessageNavigationUrl(0, oContext, NoticeBoardContext.ViewModeType.DashBoard, NoticeBoardContext.SmallViewType.None)
        '    Else
        '        Me.View.SetPreviousURL = ""
        '    End If
        '    Me.ShowMessage(oNoticeboard, CommunityID, isCurrent, oContext)
        'End Sub


        'Private Function GetPreviousMessage(ByVal oContext As NoticeBoardContext, ByVal oDeleted As NoticeBoard) As NoticeBoard
        '    Dim oPreviousNoticeboard As NoticeBoard
        '    Dim oMessages As IEnumerable(Of NoticeBoard)
        '    Dim oCommunity As Community = oDeleted.CommunityOwner
        '    Dim PageSize As Integer
        '    Dim PageIndex As Integer = oContext.PageIndex

        '    If oContext.SmallView = NoticeBoardContext.SmallViewType.LastFourMessage Then
        '        PageSize = 5
        '    Else
        '        PageSize = 10
        '    End If

        '    Select Case oContext.SmallView
        '        Case NoticeBoardContext.SmallViewType.AllMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '        Case NoticeBoardContext.SmallViewType.LastFourMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '        Case NoticeBoardContext.SmallViewType.AlsoPreviousMessages
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '        Case Else
        '            oMessages = New List(Of NoticeBoard)
        '    End Select


        '    Dim oList As List(Of NoticeBoard)
        '    Dim oItemIndex As Integer
        '    oList = oMessages.Skip(PageIndex * PageSize).Take(PageSize).ToList()
        '    oItemIndex = oList.IndexOf(oDeleted)
        '    If oItemIndex = 0 AndAlso oList.Count > 1 Then
        '        oPreviousNoticeboard = oList(1)
        '    ElseIf oItemIndex > 0 Then
        '        oPreviousNoticeboard = oList(oItemIndex - 1)
        '    Else
        '        oPreviousNoticeboard = LastMessage(oContext.CommunityID)
        '    End If
        '    Return oPreviousNoticeboard
        'End Function
        'Private Function GetPreviousMessageID(ByVal oContext As NoticeBoardContext, ByVal oDeleted As NoticeBoard) As Long
        '    Dim PreviousID As Long
        '    Dim oMessages As IEnumerable(Of Long)
        '    Dim oCommunity As Community = oDeleted.CommunityOwner
        '    Dim PageSize As Integer
        '    Dim PageIndex As Integer = oContext.PageIndex

        '    If oContext.SmallView = NoticeBoardContext.SmallViewType.LastFourMessage Then
        '        PageSize = 5
        '    Else
        '        PageSize = 10
        '    End If

        '    Select Case oContext.SmallView
        '        Case NoticeBoardContext.SmallViewType.AllMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n.Id)
        '        Case NoticeBoardContext.SmallViewType.LastFourMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n.Id)
        '        Case NoticeBoardContext.SmallViewType.AlsoPreviousMessages
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n.Id)
        '        Case Else
        '            oMessages = New List(Of NoticeBoard)
        '    End Select


        '    Dim oList As List(Of Long)
        '    Dim oItemIndex As Integer
        '    oList = oMessages.Skip(PageIndex * PageSize).Take(PageSize).ToList()
        '    oItemIndex = oList.IndexOf(oDeleted.Id)
        '    If oItemIndex = 0 AndAlso oList.Count > 1 Then
        '        PreviousID = oList(1)
        '    ElseIf oItemIndex > 0 Then
        '        PreviousID = oList(oItemIndex - 1)
        '    Else
        '        PreviousID = 0
        '    End If
        '    Return PreviousID
        'End Function
        'Private Sub ShowMessage(ByVal oNoticeboard As NoticeBoard, ByVal CommunityID As Integer, ByVal isCurrent As Boolean, ByVal oContext As NoticeBoardContext)
        '    If HasPermissionToView(CommunityID) Then
        '        Dim CommunityName As String = ""
        '        If CommunityID > 0 Then
        '            Dim oCommunity As Community = Me.CommonManager.GetCommunity(CommunityID)
        '            If Not IsNothing(oCommunity) Then
        '                CommunityName = oCommunity.Name
        '            End If
        '            Me.View.setHeaderTitle("community")
        '        Else
        '            Me.View.setHeaderTitle("")
        '        End If
        '        Me.View.CurrentMessageID = oNoticeboard.Id
        '        Me.View.ViewMessage(oNoticeboard, Me.Permission(CommunityID), isCurrent, CommunityID, Me.UserContext.CurrentUserID, CommunityName)
        '        Me.RetrievePreviousMessage(CommunityID, oContext)

        '        Dim tmp As NoticeBoardContext = oContext.Clone
        '        If oContext.ViewMode = NoticeBoardContext.ViewModeType.NewMessageADV OrElse oContext.ViewMode = NoticeBoardContext.ViewModeType.NewMessageHTML Then
        '            tmp.PreviousView = NoticeBoardContext.ViewModeType.None
        '        Else
        '            tmp.PreviousView = tmp.ViewMode
        '        End If

        '        Dim HTMLurl As String = Me.View.GetNavigationUrl(tmp, NoticeBoardContext.ViewModeType.NewMessageHTML, tmp.SmallView)
        '        Dim AdvancedUrl As String = Me.View.GetNavigationUrl(tmp, NoticeBoardContext.ViewModeType.NewMessageADV, tmp.SmallView)
        '        Me.View.SetNewMessageUrl(HTMLurl, AdvancedUrl)
        '    Else
        '        Me.View.AddActionNoPermission(CommunityID, ModuleID, Me.UserContext.CurrentUserID)
        '        Me.View.NoPermissionToAccess()
        '    End If
        'End Sub
        'Private Function HasPermissionToView(ByVal CommunityID As Integer) As Boolean
        '    Dim oPermission As ModuleNoticeBoard = Me.Permission(CommunityID)
        '    Return oPermission.ViewCurrentMessage OrElse oPermission.ViewOldMessage OrElse oPermission.ServiceAdministration
        'End Function

        'Private Function LastMessage(ByVal CommunityID As Integer) As NoticeBoard
        '    Dim oNoticeboard As NoticeBoard
        '    Dim oCommunity As Community = DataContext.GetById(Of Community)(CommunityID)
        '    oNoticeboard = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n).FirstOrDefault

        '    Return oNoticeboard
        'End Function

        'Private Sub RetrievePreviousMessage(ByVal CommunityId As Integer, ByVal oContext As NoticeBoardContext)
        '    Dim oList As New List(Of dtoSmallMessage)
        '    Dim oMessages As IEnumerable(Of NoticeBoard)
        '    Dim oCommunity As Community = DataContext.GetById(Of Community)(CommunityId)

        '    Dim oPager As PagerBase
        '    oPager = Me.View.Pager
        '    oPager.PageIndex = 0
        '    oPager.Count = -1
        '    If oContext.SmallView = NoticeBoardContext.SmallViewType.LastFourMessage Then
        '        oPager.PageSize = 5
        '    Else
        '        oPager.PageSize = 10
        '    End If

        '    Select Case oContext.SmallView
        '        Case NoticeBoardContext.SmallViewType.AllMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '            oPager.Count = oMessages.Count - 1
        '            oPager.PageIndex = oContext.PageIndex

        '        Case NoticeBoardContext.SmallViewType.LastFourMessage
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '            oPager.Count = oMessages.Count - 1
        '            If oPager.Count > 4 Then
        '                oPager.Count = 4
        '            End If
        '        Case NoticeBoardContext.SmallViewType.AlsoPreviousMessages
        '            oMessages = (From n In DataContext.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n)
        '            oPager.Count = oMessages.Count - 1
        '            oPager.PageIndex = oContext.PageIndex
        '        Case Else
        '            oMessages = New List(Of NoticeBoard)
        '    End Select

        '    oMessages = oMessages.Skip(oPager.Skip).Take(oPager.PageSize)

        '    oContext.UserID = 0
        '    oContext.ViewMode = NoticeBoardContext.ViewModeType.Message
        '    oList = (From n In oMessages Select New dtoSmallMessage(n) With {.Url = Me.View.GetMessageNavigationUrl(n.Id, oContext, oContext.ViewMode, oContext.SmallView)}).ToList

        '    Me.View.NavigationUrl(oContext, oContext.ViewMode, oContext.SmallView)
        '    Me.View.Pager = oPager
        '    Me.View.ViewPreviousMessages(oList, Me.Permission(CommunityId), oContext.SmallView)
        'End Sub

        'Public Function GetMoreNotdeletedUrl() As String
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    oContext.UserID = 0
        '    oContext.MessageID = Me.View.CurrentMessageID
        '    oContext.SmallView = NoticeBoardContext.SmallViewType.AllMessage
        '    Return Me.View.GetNavigationUrl(oContext, oContext.ViewMode, NoticeBoardContext.SmallViewType.AllMessage)
        'End Function
        'Public Function GetMoreMessageUrl() As String
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone

        '    oContext.UserID = 0
        '    oContext.MessageID = Me.View.CurrentMessageID
        '    oContext.SmallView = NoticeBoardContext.SmallViewType.AlsoPreviousMessages
        '    Return Me.View.GetNavigationUrl(oContext, oContext.ViewMode, NoticeBoardContext.SmallViewType.AlsoPreviousMessages)
        'End Function
        'Public Function GetLastFiveMessageUrl() As String
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone

        '    oContext.UserID = 0
        '    oContext.MessageID = Me.View.CurrentMessageID
        '    oContext.SmallView = NoticeBoardContext.SmallViewType.LastFourMessage
        '    Return Me.View.GetNavigationUrl(oContext, oContext.ViewMode, NoticeBoardContext.SmallViewType.LastFourMessage)
        'End Function
        'Public Sub Save(ByVal oNoticeboard As NoticeBoard)
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim RegisterAction As Boolean = True
        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    If IsNothing(oNoticeboard) Then
        '        Me.View.CurrentMessageID = oContext.MessageID
        '        Me.LoadMessage(oContext, RegisterAction)
        '    Else
        '        Dim ToSave As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(oNoticeboard.Id)
        '        Try
        '            DataContext.BeginTransaction()
        '            If Not IsNothing(ToSave) AndAlso ToSave.Message <> oNoticeboard.Message Then
        '                ToSave = New NoticeBoard
        '            End If
        '            If IsNothing(ToSave) OrElse ToSave.Id = 0 Then
        '                ToSave = New NoticeBoard
        '                ToSave.CreatedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
        '                ToSave.CreatedOn = Now
        '                ToSave.isDeleted = False
        '                ToSave.ModifiedBy = ToSave.CreatedBy
        '                ToSave.ModifiedOn = ToSave.CreatedOn
        '                ToSave.Owner = ToSave.CreatedBy
        '            Else
        '                ToSave.ModifiedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
        '                ToSave.ModifiedOn = Now
        '            End If
        '            ToSave.CommunityOwner = Me.DataContext.GetById(Of Community)(Me.View.MessageCommunityID)
        '            ToSave.CreateByAdvancedEditor = oNoticeboard.CreateByAdvancedEditor
        '            If ToSave.Id = 0 Then
        '                ToSave.isForPortal = (Me.View.MessageCommunityID = 0)
        '                If Not IsNothing(oNoticeboard.Style) Then
        '                    Dim oStyle As New TextStyle
        '                    oStyle = oNoticeboard.Style
        '                    Me.DataContext.SaveOrUpdate(oStyle)
        '                    ToSave.Style = oStyle
        '                End If
        '            Else
        '                If ToSave.CreateByAdvancedEditor And Not IsNothing(ToSave.Style) Then
        '                    Me.DataContext.Delete(ToSave.Style)
        '                    ToSave.Style = Nothing
        '                ElseIf ToSave.CreateByAdvancedEditor = False Then
        '                    Dim oStyle As New TextStyle
        '                    If IsNothing(ToSave.Style) Then
        '                        oStyle = oNoticeboard.Style
        '                        Me.DataContext.SaveOrUpdate(oStyle)
        '                        ToSave.Style = oStyle
        '                    Else
        '                        ToSave.Style.Align = oNoticeboard.Style.Align
        '                        ToSave.Style.BackGround = oNoticeboard.Style.BackGround
        '                        ToSave.Style.Color = oNoticeboard.Style.Color
        '                        ToSave.Style.Face = oNoticeboard.Style.Face
        '                        ToSave.Style.Size = oNoticeboard.Style.Size
        '                        Me.DataContext.SaveOrUpdate(ToSave.Style)
        '                    End If
        '                End If
        '            End If
        '            ToSave.Message = oNoticeboard.Message
        '            If HasPermissionToEdit(ToSave.CommunityOwner) Then
        '                Me.DataContext.SaveOrUpdate(ToSave)
        '                DataContext.Commit()
        '                Me.View.CurrentMessageID = ToSave.Id
        '                oContext.MessageID = ToSave.Id

        '                RegisterAction = False
        '                Dim Values As List(Of String) = Me.GetNotificationValues(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)
        '                Dim UrlToNotify As String = Me.GetNotificationUrl(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)
        '                Dim CommunityID As Integer
        '                If ToSave.CommunityOwner Is Nothing Then
        '                    CommunityID = 0
        '                Else
        '                    CommunityID = ToSave.CommunityOwner.Id
        '                End If
        '                If oNoticeboard.Id = 0 Then
        '                    Me.View.AddCreateMessage(ToSave.Id, ModuleID, CommunityID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '                Else
        '                    Me.View.AddEditAction(ToSave.Id, ModuleID, CommunityID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '                End If

        '            Else
        '                DataContext.Rollback()
        '            End If
        '        Catch ex As Exception
        '            DataContext.Rollback()
        '        End Try
        '        Me.LoadMessage(oContext, RegisterAction)
        '    End If
        'End Sub

        'Public Sub DeleteMessage()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    Dim ToSave As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(Me.View.CurrentMessageID)
        '    If Not IsNothing(ToSave) AndAlso ToSave.Id > 0 Then
        '        Dim PreviousMessageID As Long = GetPreviousMessageID(oContext, ToSave)
        '        Try
        '            Dim NotificationID As Long = ToSave.Id
        '            Dim CommunityID As Long = 0
        '            If Not IsNothing(ToSave.CommunityOwner) Then
        '                CommunityID = ToSave.CommunityOwner.Id
        '            End If
        '            ' Dim UrlToMessage As String = Me.GetNotificationUrl(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.AllMessage)

        '            DataContext.BeginTransaction()

        '            Me.DataContext.Delete(ToSave)
        '            DataContext.Commit()
        '            RetrievePreviousMessage(CommunityID, oContext)


        '            Me.View.AddDeleteAction(NotificationID, CommunityID, ModuleID, Now, Me.UserContext.CurrentUser.SurnameAndName)
        '        Catch ex As Exception
        '            If DataContext.isInTransaction Then
        '                DataContext.Rollback()
        '            End If
        '        End Try
        '        oContext.MessageID = PreviousMessageID
        '    End If
        '    Me.LoadMessage(oContext, True)
        'End Sub
        'Public Sub EditMessage(ByVal asHTML As Boolean)
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim ToEdit As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(Me.View.CurrentMessageID)
        '    Dim CommunityID As Integer = oContext.CommunityID

        '    If IsNothing(ToEdit) Then
        '        Me.LoadMessage(oContext, True)
        '    Else
        '        Me.View.CurrentMessageID = ToEdit.Id
        '        If ToEdit.CommunityOwner Is Nothing OrElse ToEdit.CommunityOwner.Id = 0 Then
        '            CommunityID = 0
        '        Else
        '            CommunityID = ToEdit.CommunityOwner.Id
        '        End If
        '        If Me.View.PreLoadedPreviousView = NoticeBoardContext.ViewModeType.None Then
        '            Me.View.SetBackUrlFromEditor = Me.View.GetNavigationUrl(oContext, Me.View.CurrentView, oContext.SmallView)
        '        Else
        '            Me.View.SetBackUrlFromEditor = Me.View.GetNavigationUrl(oContext, Me.View.PreLoadedPreviousView, oContext.SmallView)
        '        End If
        '        Me.View.EditMessage(ToEdit, asHTML)
        '    End If
        '    If CommunityID = 0 Then
        '        Me.View.setHeaderTitle("")
        '    Else
        '        Me.View.setHeaderTitle("community")
        '    End If
        'End Sub
        'Public Sub UnDeleteMessage(ByVal Ripristina As Boolean)
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim RegisterAction As Boolean = True
        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    Dim ToSave As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(Me.View.CurrentMessageID)
        '    If Not IsNothing(ToSave) AndAlso ToSave.Id > 0 Then
        '        Try
        '            DataContext.BeginTransaction()
        '            If Ripristina Then
        '                ToSave.ModifiedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
        '                ToSave.ModifiedOn = Now
        '            End If
        '            ToSave.isDeleted = False
        '            DataContext.Commit()

        '            Dim CommunityID As Long = 0
        '            If Not IsNothing(ToSave.CommunityOwner) Then
        '                CommunityID = ToSave.CommunityOwner.Id
        '            End If
        '            RetrievePreviousMessage(CommunityID, oContext)

        '            RegisterAction = False
        '            Dim UrlToNotify As String = GetNotificationUrl(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)
        '            If Ripristina Then
        '                Me.View.AddUndeleteActionAndActivate(ToSave.Id, CommunityID, ModuleID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '            Else
        '                Me.View.AddUndeleteAction(ToSave.Id, CommunityID, ModuleID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '            End If
        '        Catch ex As Exception
        '            DataContext.Rollback()
        '        End Try
        '    End If
        '    Me.LoadMessage(oContext, RegisterAction)
        'End Sub
        'Public Sub VirtualDeleteMessage()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim RegisterAction As Boolean = True

        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    Dim ToSave As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(Me.View.CurrentMessageID)
        '    If Not IsNothing(ToSave) AndAlso ToSave.Id > 0 Then

        '        Try
        '            DataContext.BeginTransaction()
        '            ToSave.ModifiedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
        '            ToSave.ModifiedOn = Now
        '            ToSave.isDeleted = True
        '            Me.DataContext.Save(ToSave)
        '            DataContext.Commit()
        '            If oContext.SmallView <> NoticeBoardContext.SmallViewType.AllMessage Then
        '                oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '            End If
        '            Dim CommunityID As Long = 0
        '            If Not IsNothing(ToSave.CommunityOwner) Then
        '                CommunityID = ToSave.CommunityOwner.Id
        '            End If
        '            RetrievePreviousMessage(CommunityID, oContext)

        '            RegisterAction = False
        '            Dim UrlToNotify As String = GetNotificationUrl(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.AllMessage)
        '            Me.View.AddVirtualDeleteAction(ToSave.Id, CommunityID, ModuleID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '        Catch ex As Exception
        '            DataContext.Rollback()
        '        End Try
        '    End If

        '    Me.LoadMessage(oContext, RegisterAction)
        'End Sub
        'Public Sub ClearNoticeBoard()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim RegisterAction As Boolean = True
        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    'Dim ToSave As NoticeBoard = New NoticeBoard
        '    ' Try
        '    'DataContext.BeginTransaction()

        '    'ToSave.CreatedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
        '    'ToSave.CreatedOn = Now
        '    'ToSave.isDeleted = False
        '    'ToSave.ModifiedBy = ToSave.CreatedBy
        '    'ToSave.ModifiedOn = ToSave.CreatedOn
        '    'ToSave.Owner = ToSave.CreatedBy

        '    'ToSave.CommunityOwner = Me.DataContext.GetById(Of Community)(#Me.UserContext.CurrentCommunityID#)
        '    'ToSave.CreateByAdvancedEditor = True
        '    'ToSave.isForPortal = (#Me.UserContext.CurrentCommunityID# = 0)
        '    'ToSave.Message = ""

        '    Dim oPermission As ModuleNoticeBoard = Me.Permission(Me.View.MessageCommunityID)
        '    If oPermission.EditMessage OrElse oPermission.ServiceAdministration Then
        '        Dim ToSave As NoticeBoard = Me.CurrentManager.ClearNoticeBoard(Me.View.MessageCommunityID, Me.UserContext.CurrentUserID, Me.View.MessageCommunityID = 0)
        '        If Not IsNothing(ToSave) Then
        '            Me.View.CurrentMessageID = ToSave.Id
        '            oContext.MessageID = ToSave.Id
        '            RegisterAction = False

        '            Dim CommunityID As Long = 0
        '            If Not IsNothing(ToSave.CommunityOwner) Then
        '                CommunityID = ToSave.CommunityOwner.Id
        '            End If
        '            RetrievePreviousMessage(CommunityID, oContext)
        '            '  Dim oValues As List(Of String) = GetNotificationValues(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)
        '            Me.View.AddCleanAction(ToSave.Id, CommunityID, Nothing)
        '        End If


        '        'Else
        '        '    DataContext.Rollback()
        '    End If

        '    'Catch ex As Exception
        '    '    DataContext.Rollback()
        '    'End Try
        '    Me.LoadMessage(oContext, RegisterAction)
        'End Sub

        'Public Sub SetActive()
        '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
        '    Dim RegisterAction As Boolean = True
        '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
        '        oContext.ViewMode = oContext.PreviousView
        '    Else
        '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
        '    End If
        '    Dim ToSetActive As NoticeBoard = Me.CurrentManager.GetMessage(Me.View.CurrentMessageID)
        '    If Not IsNothing(ToSetActive) Then
        '        If HasPermissionToEdit(ToSetActive.CommunityOwner) Then
        '            Dim ToSave As NoticeBoard = Me.CurrentManager.SetActive(Me.View.CurrentMessageID, Me.UserContext.CurrentUserID)
        '            If Not IsNothing(ToSave) Then
        '                RegisterAction = False
        '                Dim UrlToNotify As String = Me.GetNotificationUrl(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)
        '                Dim CommunityID As Integer
        '                If ToSave.CommunityOwner Is Nothing Then
        '                    CommunityID = 0
        '                Else
        '                    CommunityID = ToSave.CommunityOwner.Id
        '                End If

        '                Me.View.AddCreateMessage(ToSave.Id, ModuleID, CommunityID, ToSave.ModifiedOn, ToSave.ModifiedBy.SurnameAndName, UrlToNotify)
        '            End If
        '        End If

        '    End If
        '    Me.LoadMessage(oContext, RegisterAction)
        'End Sub
        'Private Function HasPermissionToEdit(ByVal oCommunity As Community) As Boolean
        '    Dim oPermission As ModuleNoticeBoard
        '    If oCommunity Is Nothing Then
        '        oPermission = Me.Permission(0)
        '    Else
        '        oPermission = Me.Permission(oCommunity.Id)
        '    End If

        '    Return oPermission.EditMessage OrElse oPermission.ServiceAdministration
        'End Function

        'Private Function GetNotificationValues(ByVal oNoticeboard As NoticeBoard, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As List(Of String)
        '    Dim Values As New List(Of String)

        '    If Not IsNothing(oNoticeboard) Then
        '        If oNoticeboard.Id > 0 Then
        '            Dim oContext As New NoticeBoardContext
        '            If oNoticeboard.CommunityOwner Is Nothing Then
        '                oContext.CommunityID = 0
        '            Else
        '                oContext.CommunityID = oNoticeboard.CommunityOwner.Id
        '            End If
        '            oContext.isPortalCommunity = oNoticeboard.isForPortal
        '            oContext.MessageID = oNoticeboard.Id
        '            oContext.PageIndex = 0
        '            oContext.PreviousView = NoticeBoardContext.ViewModeType.None
        '            oContext.SmallView = oSmallView
        '            oContext.UserID = 0
        '            oContext.ViewMode = oView

        '            If oNoticeboard.isDeleted Then
        '                Values.Add(FormatDateTime(oNoticeboard.ModifiedOn, DateFormat.LongDate))
        '                If oNoticeboard.ModifiedOn.HasValue Then
        '                    Values.Add(oNoticeboard.ModifiedOn.Value.ToString)
        '                Else
        '                    Values.Add("")
        '                End If
        '                Values.Add(oNoticeboard.ModifiedBy.SurnameAndName)
        '            Else
        '                Values.Add(Me.View.GetMessageNavigationUrl(oNoticeboard.Id, oContext, oView, oSmallView))
        '                If oNoticeboard.ModifiedOn.HasValue Then
        '                    Values.Add(oNoticeboard.ModifiedOn.Value.ToString)
        '                Else
        '                    Values.Add("")
        '                End If
        '                Values.Add(oNoticeboard.ModifiedBy.SurnameAndName)
        '            End If
        '        End If
        '    End If
        '    Return Values
        'End Function

        'Private Function GetNotificationUrl(ByVal oNoticeboard As NoticeBoard, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As String
        '    Dim iResponse As String = ""

        '    Dim MessageID As Long = 0
        '    Dim oContext As New NoticeBoardContext
        '    If Not IsNothing(oNoticeboard) Then
        '        If oNoticeboard.CommunityOwner Is Nothing Then
        '            oContext.CommunityID = 0
        '        Else
        '            oContext.CommunityID = oNoticeboard.CommunityOwner.Id
        '        End If
        '        MessageID = oNoticeboard.Id
        '    Else
        '        oContext.CommunityID = Me.View.MessageCommunityID
        '    End If
        '    oContext.isPortalCommunity = oNoticeboard.isForPortal
        '    oContext.MessageID = oNoticeboard.Id
        '    oContext.PageIndex = 0
        '    oContext.PreviousView = NoticeBoardContext.ViewModeType.None
        '    oContext.SmallView = oSmallView
        '    oContext.UserID = 0
        '    oContext.ViewMode = oView

        '    iResponse = Me.View.GetMessageNavigationUrl(MessageID, oContext, oView, oSmallView)
        '    Return iResponse
        'End Function
    End Class
End Namespace