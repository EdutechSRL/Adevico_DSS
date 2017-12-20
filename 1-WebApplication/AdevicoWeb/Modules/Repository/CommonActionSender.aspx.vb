Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.UI.Presentation
Public Class CommonActionSender
    Inherits System.Web.UI.Page
    Implements IViewCommonActionSender

#Region "Context"
    Private _Presenter As CommonActionSenderPresenter
    Private _serviceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service

    Private _serviceFile As lm.Comol.Core.FileRepository.Business.ServiceFileRepository

    Public ReadOnly Property CurrentPresenter() As CommonActionSenderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommonActionSenderPresenter(New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private ReadOnly Property ServiceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
        Get
            If IsNothing(_serviceEduPath) Then
                _serviceEduPath = New lm.Comol.Modules.EduPath.BusinessLogic.Service(PageUtility.CurrentContext)
            End If
            Return _serviceEduPath
        End Get
    End Property



    Private ReadOnly Property ServiceFile As lm.Comol.Core.FileRepository.Business.ServiceFileRepository
        Get
            If IsNothing(_serviceFile) Then
                _serviceFile = New lm.Comol.Core.FileRepository.Business.ServiceFileRepository(PageUtility.CurrentContext)
            End If
            Return _serviceFile
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Protected Friend ReadOnly Property PreloadIsModal As Boolean Implements IViewCommonActionSender.PreloadIsOnModal
        Get
            Return GetBooleanFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.onModalPage, False)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadRefreshContainer As Boolean Implements IViewCommonActionSender.PreloadRefreshContainer
        Get
            Return GetBooleanFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.refreshContainer, False)
        End Get
    End Property
  
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewCommonActionSender.PreloadIdCommunity
        Get
            Return GetIntegerFromQueryString("idCommunity", 0)
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIdItem As Long Implements IViewCommonActionSender.PreloadIdItem
        Get
            Return GetLongFromQueryString("idItem", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdVersion As Long Implements IViewCommonActionSender.PreloadIdVersion
        Get
            Return GetLongFromQueryString("idVersion", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdLink As Long Implements IViewCommonActionSender.PreloadIdLink
        Get
            Return GetLongFromQueryString("idLink", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdAction As Long Implements IViewCommonActionSender.PreloadIdAction
        Get
            Return GetLongFromQueryString("idAction", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadType As ItemType Implements IViewCommonActionSender.PreloadItemType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemType).GetByString(Request.QueryString("type"), ItemType.None)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadWorkingSessionId As Guid Implements IViewCommonActionSender.PreloadWorkingSessionId
        Get
            Return GetGuidFromQueryString("wSessionId", Guid.Empty)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadPlaySessionId As String Implements IViewCommonActionSender.PreloadPlaySessionId
        Get
            Return GetStringFromQueryString("pSessionId", "")
        End Get
    End Property

#End Region

#Region "Context"
    Protected Friend Property IdItem As Long Implements IViewCommonActionSender.IdItem
        Get
            Return ViewStateOrDefault("IdItem", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdItem") = value
        End Set
    End Property
    Protected Friend Property IdVersion As Long Implements IViewCommonActionSender.IdVersion
        Get
            Return ViewStateOrDefault("IdVersion", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdVersion") = value
        End Set
    End Property
    Protected Friend Property IdLink As Long Implements IViewCommonActionSender.IdLink
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property
   
    Protected Friend Property IdAction As Long Implements IViewCommonActionSender.IdAction
        Get
            Return ViewStateOrDefault("IdAction", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAction") = value
        End Set
    End Property
    Protected Friend Property ItemType As ItemType Implements IViewCommonActionSender.ItemType
        Get
            Return ViewStateOrDefault("ItemType", ItemType.None)
        End Get
        Set(value As ItemType)
            ViewState("ItemType") = value
        End Set
    End Property
    Protected Friend Property UniqueId As Guid Implements IViewCommonActionSender.UniqueId
        Get
            Return ViewStateOrDefault("UniqueId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("UniqueId") = value
        End Set
    End Property
    Protected Friend Property UniqueIdVersion As Guid Implements IViewCommonActionSender.UniqueIdVersion
        Get
            Return ViewStateOrDefault("UniqueIdVersion", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("UniqueIdVersion") = value
        End Set
    End Property
    Protected Friend Property WorkingSessionId As Guid Implements IViewCommonActionSender.WorkingSessionId
        Get
            Return ViewStateOrDefault("WorkingSessionId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("WorkingSessionId") = value
        End Set
    End Property
    Protected Friend Property PlaySessionId As String Implements IViewCommonActionSender.PlaySessionId
        Get
            Dim session As String = ViewStateOrDefault("PlaySessionId", "")
            If String.IsNullOrWhiteSpace(session) Then
                session = PageUtility.SystemSettings.PlattformId & "_" & Guid.NewGuid.ToString
                ViewState("PlaySessionId") = session
            End If
            Return session
        End Get
        Set(ByVal value As String)
            Me.ViewState("PlaySessionId") = value
        End Set
    End Property
    Protected Friend Property ConnectionString As String Implements IViewCommonActionSender.ConnectionString
        Get
            Return ViewStateOrDefault("ConnectionString", "")
        End Get
        Set(value As String)
            ViewState("ConnectionString") = value
        End Set
    End Property

#End Region
#End Region

#Region "Internal"
    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Context)
            End If
            Return _PageUtility
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CurrentPresenter.InitView(PreloadIdItem, PreloadIdVersion, PreloadIdLink)
        End If
    End Sub


#Region "Implements"
    Private Sub InitializeContext(iIdItem As Long, iIdVersion As Long, iIdLink As Long, itemUniqueId As Guid, itemUniqueIdVersion As Guid) Implements IViewCommonActionSender.InitializeContext
        IdItem = iIdItem
        IdVersion = iIdVersion
        IdAction = PreloadIdAction
        UniqueId = itemUniqueId
        UniqueIdVersion = itemUniqueIdVersion
        IdLink = iIdLink
        ItemType = PreloadType
        WorkingSessionId = PreloadWorkingSessionId
        InitTimer()
    End Sub
    Private Sub StopTimer() Implements IViewCommonActionSender.StopTimer
        TMSessione.Enabled = False
    End Sub

#End Region

#Region "Internal"

#Region "Timer"
    Public Sub InitTimer()
        Me.TMSessione.Interval = PageUtility.SystemSettings.Presenter.AjaxTimer
    End Sub

    Private Sub UpdateAction()
        If PageUtility.UniqueGuidSession = WorkingSessionId Then
            'If Me.PreLoadedModuleID <> 0 AndAlso Me.PreLoadedActionID > 0 AndAlso Me.PreLoadedCommunityID > -1 Then
            '    Me.PageUtility.AddActionToModule(Me.PreLoadedCommunityID, Me.PreLoadedModuleID, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
            'End If
        Else
            TMSessione.Enabled = False
        End If
        'Me.PageUtility.AddAction(Services_File.ActionType.AjaxUpdate, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Protected Sub SMTimer_AsyncPostBackError(ByVal sender As Object, ByVal e As AsyncPostBackErrorEventArgs) Handles SMTimer.AsyncPostBackError
        SMTimer.AsyncPostBackErrorMessage = e.Exception.Message + e.Exception.StackTrace
    End Sub



    Private Sub TMSessione_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMSessione.Tick
        TMSessione.Enabled = Not PageUtility.CurrentContext.UserContext.isAnonymous
    End Sub

#End Region
#Region "QueryString"
    Protected Friend ReadOnly Property GetIntegerFromQueryString(key As String, dValue As Integer) As Integer
        Get
            Dim idItem As Integer = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Integer.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Protected Friend ReadOnly Property GetLongFromQueryString(key As String, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Long.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Protected Friend ReadOnly Property GetBooleanFromQueryString(key As String, dValue As Boolean) As Boolean
        Get
            Dim result As Boolean = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Boolean.TryParse(Request.QueryString(key), result)
            End If
            Return result
        End Get
    End Property
    Protected Friend ReadOnly Property GetGuidFromQueryString(key As String, dValue As Guid) As Guid
        Get
            Dim result As Guid = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Guid.TryParse(Request.QueryString(key), result)
            End If
            Return result
        End Get
    End Property
    Protected Friend ReadOnly Property GetStringFromQueryString(key As String, dValue As String) As String
        Get
            Dim result As String = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                result = Request.QueryString(key)
            End If
            Return result
        End Get
    End Property
#End Region

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

#End Region


End Class