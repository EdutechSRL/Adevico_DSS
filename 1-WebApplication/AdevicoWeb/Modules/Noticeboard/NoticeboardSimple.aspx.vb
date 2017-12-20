Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.ActionDataContract

Partial Public Class NoticeboardSimple
    Inherits Page
    '    Inherits PageBase
    '    Implements IViewNoticeBoardEdit

    '#Region "Implements"
    '    Private _BaseUrl As String
    '    Private _PageUtility As OLDpageUtility
    '    Private _CommunitiesPermission As IList(Of CommunityDiaryCommunityPermission)
    '    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    '    Private _Presenter As NoticeBoardEditPresenter
    '    Private _CommunityRepositoryPermission As List(Of ModuleCommunityRepository)

    '    Public ReadOnly Property CurrentPresenter() As NoticeBoardEditPresenter
    '        Get
    '            If IsNothing(_Presenter) Then
    '                _Presenter = New NoticeBoardEditPresenter(Me.CurrentContext, Me)
    '            End If
    '            Return _Presenter
    '        End Get
    '    End Property
    '    'Protected ReadOnly Property PageUtility() As OLDpageUtility
    '    '    Get
    '    '        If IsNothing(_PageUtility) Then
    '    '            _PageUtility = New OLDpageUtility(Me.Context)
    '    '        End If
    '    '        Return _PageUtility
    '    '    End Get
    '    'End Property
    '    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
    '        Get
    '            If IsNothing(_CurrentContext) Then
    '                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
    '            End If
    '            Return _CurrentContext
    '        End Get
    '    End Property
    '    Public Function NoticeboardPermission(ByVal CommunityID As Integer) As ModuleNoticeBoard Implements IViewNoticeBoardEdit.NoticeboardPermission
    '        Dim oModule As ModuleNoticeBoard = Nothing

    '        If CommunityID = 0 Then
    '            oModule = ModuleNoticeBoard.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
    '        Else
    '            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_Bacheca.Codex) _
    '                         Where sb.CommunityID = CommunityID Select New ModuleNoticeBoard(New Services_Bacheca(sb.PermissionString))).FirstOrDefault
    '        End If
    '        If IsNothing(oModule) Then
    '            oModule = New ModuleNoticeBoard
    '        End If

    '        Return oModule
    '    End Function

    '    Public Property CurrentMessageID() As Long Implements IViewNoticeBoardEdit.CurrentMessageID
    '        Get
    '            Dim MessageID As Long = 0
    '            Try
    '                MessageID = Me.ViewState("CurrentMessageID")
    '            Catch ex As Exception

    '            End Try
    '            Return MessageID
    '        End Get
    '        Set(ByVal value As Long)
    '            Me.ViewState("CurrentMessageID") = value
    '        End Set
    '    End Property

    '    Public ReadOnly Property PreLoadedMessagesToShow() As MessagesToShow Implements IViewNoticeBoardEdit.PreLoadedMessagesToShow
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MessagesToShow).GetByString(Me.Request.QueryString("MessagesToShow"), MessagesToShow.LastFourMessage)
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedPage() As NoticeBoardContext.ViewModeType Implements IViewNoticeBoardEdit.PreLoadedPage
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("Page"), NoticeBoardContext.ViewModeType.None)
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedMessageID() As Long Implements IViewNoticeBoardEdit.PreLoadedMessageID
    '        Get
    '            Dim MessageID As Long = 0
    '            Try
    '                MessageID = CLng(Me.Request.QueryString("MessageID"))
    '            Catch ex As Exception

    '            End Try
    '            Return MessageID
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IViewNoticeBoardEdit.PreLoadedCommunityID
    '        Get
    '            Dim CommunityID As Integer = -1
    '            Try
    '                CommunityID = CLng(Me.Request.QueryString("CommunityID"))
    '            Catch ex As Exception

    '            End Try
    '            Return CommunityID
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedPreviousPage() As NoticeBoardContext.ViewModeType Implements IViewNoticeBoardEdit.PreLoadedPreviousPage
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("PreviousPage"), NoticeBoardContext.ViewModeType.None)
    '        End Get
    '    End Property
    '    Public Property NoticeboardCommunityID() As Integer Implements IViewNoticeBoardEdit.NoticeboardCommunityID
    '        Get
    '            If TypeOf Me.ViewState("NoticeboardCommunityID") Is Integer Then
    '                Return CLng(Me.ViewState("NoticeboardCommunityID"))
    '            Else
    '                Return -1
    '            End If
    '        End Get
    '        Set(ByVal value As Integer)
    '            Me.ViewState("NoticeboardCommunityID") = value
    '        End Set
    '    End Property
    '#End Region

    '#Region "Inherits"
    '    Public Overrides ReadOnly Property AlwaysBind() As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property
    '    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
    '        Get
    '            Return True
    '        End Get
    '    End Property
    '#End Region

    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    End Sub

    '#Region "Inherits"
    '    Public Overrides Sub BindDati()
    '        Me.Master.ShowHeaderLanguageChanger = True
    '        Me.Master.ShowNoPermission = False
    '        If Page.IsPostBack = False Then
    '            Me.CurrentPresenter.InitView(False)
    '        End If
    '    End Sub
    '    Public Overrides Sub BindNoPermessi()
    '        Me.Master.ShowNoPermission = True
    '    End Sub
    '    Public Overrides Function HasPermessi() As Boolean
    '        Return True
    '    End Function
    '    Public Overrides Sub RegistraAccessoPagina()

    '    End Sub
    '    Public Overrides Sub SetCultureSettings()
    '        MyBase.SetCulture("pg_CommunityNoticeboard", "Generici")
    '    End Sub

    '    Public Overrides Sub SetInternazionalizzazione()
    '        If IsNothing(MyBase.Resource) Then
    '            Me.SetCultureSettings()
    '        End If
    '        With MyBase.Resource
    '            .setButton(BTNsaveHTML, True)
    '            .setHyperLink(HYPback, True, True)
    '        End With
    '    End Sub
    '    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    '    End Sub
    '#End Region

    '    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PersonID As Integer) Implements IViewNoticeBoardEdit.AddActionNoPermission

    '    End Sub

    '    Public Sub AddCreateAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As System.Collections.Generic.List(Of String)) Implements IViewNoticeBoardEdit.AddCreateAction

    '    End Sub

    '    Public Sub AddEditAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As System.Collections.Generic.List(Of String)) Implements IViewNoticeBoardEdit.AddEditAction

    '    End Sub

    '    Public Sub AddGenericErrorAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer) Implements IViewNoticeBoardEdit.AddGenericErrorAction

    '    End Sub


    '    Public Sub setHeaderTitle(ByVal CommunityName As String, ByVal ForEdit As Boolean, ByVal ForPortal As Boolean) Implements IViewNoticeBoardEdit.setHeaderTitle
    '        If CommunityName = "" Then
    '            Me.Master.ServiceTitle = Me.Resource.getValue("editTitle_" & ForEdit.ToString & "_" & ForPortal.ToString)
    '        Else
    '            Me.Master.ServiceTitle = Me.Resource.getValue("editCommunityTitle_" & ForEdit.ToString & "_" & ForPortal.ToString)
    '        End If
    '    End Sub

    '    Public Sub NoMessageWithThisID(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements lm.Comol.Modules.Base.Presentation.IViewNoticeBoardEdit.NoMessageWithThisID

    '    End Sub

    '    Public Sub SendNoPermissionAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewNoticeBoardEdit.SendNoPermissionAction

    '    End Sub

    '    Public Sub GotoPage(ByVal Page As NoticeBoardContext.ViewModeType, ByVal PreviousPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer) Implements IViewNoticeBoardEdit.GotoPage

    '    End Sub

    '    Public Property MessageText() As String Implements IViewNoticeBoardEdit.MessageText
    '        Get
    '            Return Me.CTRLeditorHTML.Html
    '        End Get
    '        Set(ByVal value As String)
    '            Me.CTRLeditorHTML.Html = value
    '        End Set
    '    End Property

    '    Public ReadOnly Property PortalName() As String Implements IViewNoticeBoardEdit.PortalName
    '        Get
    '            Me.Resource.getValue("PortalName")
    '        End Get
    '    End Property

    '    Public Sub SetPreviousUrl(ByVal Page As NoticeBoardContext.ViewModeType, ByVal PreviousPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer) Implements IViewNoticeBoardEdit.SetPreviousUrl
    '        Select Case Page
    '            Case NoticeBoardContext.ViewModeType.CommunityNoticeboard
    '                Me.HYPback.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardContainer.aspx?PreviousPage=" & PreviousPage.ToString & "&MessagesToShow=" & View.ToString & "&CommunityID=" & CommunityID.ToString
    '            Case NoticeBoardContext.ViewModeType.PortalNoticeboard
    '                Me.HYPback.NavigateUrl = Me.BaseUrl & "Modules/Noticeboard/NoticeboardContainer.aspx?PreviousPage=" & PreviousPage.ToString & "&MessagesToShow=" & View.ToString & "&CommunityID=" & CommunityID.ToString
    '        End Select
    '        If Me.HYPback.NavigateUrl = "" Then
    '            Me.HYPback.Visible = False
    '        Else
    '            Me.HYPback.Visible = True
    '            If MessageID > 0 Then
    '                Me.HYPback.NavigateUrl &= "&MessageID=" & MessageID.ToString
    '            End If
    '        End If
    '    End Sub

    '    Public Property MessageStyle() As TextStyle Implements IViewNoticeBoardEdit.MessageStyle
    '        Get
    '            Dim oTextStyle As New TextStyle
    '            oTextStyle.Align = Me.CTRLeditorHTML.Alignment
    '            oTextStyle.BackGround = Me.CTRLeditorHTML.BackGround
    '            oTextStyle.Color = Me.CTRLeditorHTML.FontColor
    '            oTextStyle.Face = Me.CTRLeditorHTML.FontFace
    '            oTextStyle.Size = Me.CTRLeditorHTML.FontSize
    '            Return oTextStyle
    '        End Get
    '        Set(ByVal value As TextStyle)
    '            Me.CTRLeditorHTML.Alignment = value.Align
    '            Me.CTRLeditorHTML.BackGround = value.BackGround
    '            Me.CTRLeditorHTML.FontColor = value.Color
    '            Me.CTRLeditorHTML.FontFace = value.Face
    '            Me.CTRLeditorHTML.FontSize = value.Size
    '        End Set
    '    End Property
End Class