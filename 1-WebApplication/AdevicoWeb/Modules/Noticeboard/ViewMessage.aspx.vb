Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain

Public Class ViewNoticeboardMessage
    Inherits NoticeboardPageBase
    Implements IViewViewNoticeboardMessage

#Region "Context"
    Private _Presenter As ViewNoticeboardMessagePresenter
    Public ReadOnly Property CurrentPresenter() As ViewNoticeboardMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewNoticeboardMessagePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Private WriteOnly Property AllowPrint As Boolean Implements IViewViewNoticeboardMessage.AllowPrint
        Set(value As Boolean)
            HYPprintNoticeboard.Visible = value
        End Set
    End Property
    Private Property IdLoaderCommunity As Integer Implements IViewViewNoticeboardMessage.IdLoaderCommunity
        Get
            Return ViewStateOrDefault("IdLoaderCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdLoaderCommunity") = value
        End Set
    End Property
#End Region

#Region "Internal"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(PreloadedIdMessage, PreloadedIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPprintNoticeboard, False, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region
#Region "Inherits"
    Private Sub DisplayMessage(idMessage As Long, idCommunity As Integer) Implements IViewViewNoticeboardMessage.DisplayMessage
        LTrenderNoticeboard.Text = String.Format(LTrenderNoticeboardTemplate.Text, BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessage(idMessage, idCommunity))
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewViewNoticeboardMessage.DisplaySessionTimeout
        LTrenderNoticeboard.Visible = False
        RedirectOnSessionTimeOut(RootObject.ViewMessage(PreloadedIdMessage, PreloadedIdCommunity), IdLoaderCommunity, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow)
    End Sub
    Private Sub SetHeaderTitle(isForPortal As Boolean, name As String) Implements IViewViewNoticeboardMessage.SetHeaderTitle
        If isForPortal Then
            Master.ServiceTitle = Resource.getValue("MessageHeaderTitle.True")
        ElseIf Not String.IsNullOrEmpty(name) AndAlso PreloadedIdCommunity <> ComunitaCorrenteID Then
            Master.ServiceTitle = String.Format(Resource.getValue("MessageHeaderTitle"), name)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("MessageHeaderTitle.ToolTip"), name)
        ElseIf Not String.IsNullOrEmpty(name) Then
            Master.ServiceTitle = Resource.getValue("MessageHeaderTitle.False")
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("MessageHeaderTitle.ToolTip"), name)
        Else
            Master.ServiceTitle = Resource.getValue("MessageHeaderTitle.False")
        End If
    End Sub
#End Region


End Class