Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.ActionDataContract


Partial Public Class NoticeboardAdvanced
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
    '    Public ReadOnly Property PreLoadedPage() As Integer Implements IViewNoticeBoardEdit.PreLoadedPage
    '        Get
    '            Dim PageNumber As Integer = 0
    '            Try
    '                PageNumber = CLng(Me.Request.QueryString("Page"))
    '            Catch ex As Exception

    '            End Try
    '            Return PageNumber
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedContainer() As NoticeBoardContext.ViewModeType Implements IViewNoticeBoardEdit.PreLoadedContainer
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("Container"), NoticeBoardContext.ViewModeType.None)
    '        End Get
    '    End Property
    '    Public ReadOnly Property PreLoadedFromPage() As NoticeBoardContext.ViewModeType Implements IViewNoticeBoardEdit.PreLoadedFromPage
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("FromPage"), NoticeBoardContext.ViewModeType.None)
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
    '            Me.CurrentPresenter.InitView(True)
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
    '            .setButton(BTNsaveADV, True)
    '            .setHyperLink(HYPback, True, True)
    '        End With
    '    End Sub
    '    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    '    End Sub
    '#End Region

    '    'Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PersonID As Integer) Implements IViewNoticeBoardEdit.AddActionNoPermission

    '    'End Sub

    '    Public Sub AddCreateAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As System.Collections.Generic.List(Of String)) Implements IViewNoticeBoardEdit.AddCreateAction

    '    End Sub

    '    Public Sub AddEditAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal values As System.Collections.Generic.List(Of String)) Implements IViewNoticeBoardEdit.AddEditAction

    '    End Sub

    '    'Public Sub AddGenericErrorAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer) Implements IViewNoticeBoardEdit.AddGenericErrorAction

    '    'End Sub

    '    Public Sub setHeaderTitle(ByVal CommunityName As String, ByVal ForEdit As Boolean, ByVal ForPortal As Boolean) Implements IViewNoticeBoardEdit.setHeaderTitle
    '        If CommunityName = "" Then
    '            Me.Master.ServiceTitle = Me.Resource.getValue("editTitle_" & ForEdit.ToString & "_" & ForPortal.ToString)
    '        Else
    '            Me.Master.ServiceTitle = Me.Resource.getValue("editCommunityTitle_" & ForEdit.ToString & "_" & ForPortal.ToString)
    '        End If
    '    End Sub

    '    Public Property MessageText() As String Implements IViewNoticeBoardEdit.MessageText
    '        Get
    '            Return Me.CTRLvisualEditor.HTML
    '        End Get
    '        Set(ByVal value As String)
    '            Me.CTRLvisualEditor.HTML = value
    '        End Set
    '    End Property
    '    Public ReadOnly Property PortalName() As String Implements IViewNoticeBoardEdit.PortalName
    '        Get
    '            Return Me.Resource.getValue("PortalName")
    '        End Get
    '    End Property

    '    Public Property MessageStyle() As TextStyle Implements IViewNoticeBoardEdit.MessageStyle
    '        Get
    '            Dim oTextStyle As New TextStyle
    '            oTextStyle.BackGround = Me.CTRLvisualEditor.BackGroundColor
    '            oTextStyle.Color = Me.CTRLvisualEditor.DefaultFontColor
    '            oTextStyle.Face = Me.CTRLvisualEditor.DefaultFontName
    '            oTextStyle.Size = Me.CTRLvisualEditor.DefaultFontSize
    '            Return oTextStyle
    '        End Get
    '        Set(ByVal value As TextStyle)
    '            Me.CTRLvisualEditor.BackGroundColor = value.BackGround
    '            Me.CTRLvisualEditor.DefaultFontColor = value.Color
    '            Me.CTRLvisualEditor.DefaultFontName = value.Face
    '            Me.CTRLvisualEditor.DefaultFontSize = value.Size
    '        End Set
    '    End Property

    '    Private Sub BTNsaveADV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsaveADV.Click
    '        Me.CurrentPresenter.Save(True)
    '    End Sub

    '    Public Sub GotoPage(ByVal Container As NoticeBoardContext.ViewModeType, ByVal ToPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) Implements IViewNoticeBoardEdit.GotoPage
    '        Me.HYPback.NavigateUrl = GenerateUrl(Container, ToPage, View, MessageID, CommunityID, PageIndex)
    '    End Sub

    '    Public Sub NoMessageWithThisID(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal MessageID As Long) Implements IViewNoticeBoardEdit.NoMessageWithThisID
    '        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_Bacheca.ActionType.GenericError, Me.PageUtility.CreateObjectsList(Services_Bacheca.ObjectType.WhiteBoard, MessageID), InteractionType.UserWithLearningObject)
    '    End Sub

    '    Public Sub SendNoPermissionAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal MessageID As Long) Implements IViewNoticeBoardEdit.SendNoPermissionAction
    '        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_Bacheca.ActionType.NoPermission, Me.PageUtility.CreateObjectsList(Services_Bacheca.ObjectType.WhiteBoard, MessageID), InteractionType.UserWithLearningObject)
    '        Me.BindNoPermessi()
    '    End Sub

    '    Public Sub SetPreviousUrl(ByVal Container As NoticeBoardContext.ViewModeType, ByVal ToPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) Implements IViewNoticeBoardEdit.SetPreviousUrl
    '        Dim Url As String = GenerateUrl(Container, ToPage, View, MessageID, CommunityID, PageIndex)
    '        Me.HYPback.Visible = Not String.IsNullOrEmpty(Url)
    '        Me.HYPback.NavigateUrl = Url
    '    End Sub

    '    Private Function GenerateUrl(ByVal Container As NoticeBoardContext.ViewModeType, ByVal ToPage As NoticeBoardContext.ViewModeType, ByVal View As MessagesToShow, ByVal MessageID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As String
    '        Dim iResponse As String = ""

    '        iResponse = Me.BaseUrl & "Modules/Noticeboard/NoticeboardContainer.aspx?CommunityID=" & CommunityID.ToString
    '        If MessageID > 0 Then
    '            iResponse &= "&MessageID=" & MessageID
    '        End If
    '        If ToPage <> NoticeBoardContext.ViewModeType.None Then
    '            iResponse &= "&FromPage=" & ToPage.ToString
    '        End If
    '        If View <> MessagesToShow.None Then
    '            iResponse &= "&MessagesToShow=" & View.ToString
    '        End If
    '        If PageIndex > 0 Then
    '            iResponse &= "&Page=" & PageIndex.ToString
    '        End If
    '        Return iResponse
    '    End Function

End Class