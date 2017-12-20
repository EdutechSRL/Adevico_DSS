Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.ActionDataContract

Partial Public Class CommunityNoticeBoard
    Inherits PageBase
    Implements IViewNoticeBoard

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
#End Region

#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
#End Region

#Region "View property"
    Public ReadOnly Property PreLoadedSmallView() As lm.Comol.Modules.Base.DomainModel.NoticeBoardContext.SmallViewType Implements lm.Comol.Modules.Base.Presentation.IViewNoticeBoard.PreLoadedSmallView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.SmallViewType).GetByString(Me.Request.QueryString("SmallView"), NoticeBoardContext.SmallViewType.None)
        End Get
    End Property
    Public ReadOnly Property PreLoadedView() As lm.Comol.Modules.Base.DomainModel.NoticeBoardContext.ViewModeType Implements lm.Comol.Modules.Base.Presentation.IViewNoticeBoard.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("View"), NoticeBoardContext.ViewModeType.None)
        End Get
    End Property
    Public ReadOnly Property PreLoadedMessageID() As Long Implements IViewNoticeBoard.PreLoadedMessageID
        Get
            Dim MessageID As Long = 0
            Try
                MessageID = CLng(Me.Request.QueryString("MessageID"))
            Catch ex As Exception

            End Try
            Return MessageID
        End Get
    End Property
    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IViewNoticeBoard.PreLoadedCommunityID
        Get
            Dim CommunityID As Integer = -1
            Try
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    CommunityID = CInt(Me.Request.QueryString("CommunityID"))
                End If
            Catch ex As Exception

            End Try
            Return CommunityID
        End Get
    End Property
#End Region

#Region "Generic Page Property"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            Select Case PreLoadedCommunityID
                Case 0
                    PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.NoticeboardDashboard(0, 0, True, False))
                Case -1
                    Dim idCommunity As Integer = ComunitaCorrenteID
                    PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.NoticeboardDashboard(0, idCommunity, (idCommunity = 0), False))
                Case Else
                    PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.NoticeboardDashboard(0, PreLoadedCommunityID, (PreLoadedCommunityID = 0), False))
            End Select
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
    Public ReadOnly Property PreLoadedPreviousView() As lm.Comol.Modules.Base.DomainModel.NoticeBoardContext.ViewModeType Implements lm.Comol.Modules.Base.Presentation.IViewNoticeBoard.PreLoadedPreviousView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NoticeBoardContext.ViewModeType).GetByString(Me.Request.QueryString("PreviousView"), NoticeBoardContext.ViewModeType.None)
        End Get
    End Property

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_Bacheca.Codex)
    End Sub

End Class