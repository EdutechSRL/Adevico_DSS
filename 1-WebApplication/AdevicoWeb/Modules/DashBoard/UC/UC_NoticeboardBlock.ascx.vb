Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_NoticeboardBlock
    Inherits DBbaseControl
    Implements IViewNoticeboardBlock

#Region "Context"
    Private _Presenter As NoticeboardBlockPresenter
    Private ReadOnly Property CurrentPresenter() As NoticeboardBlockPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NoticeboardBlockPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property AllowPrint As Boolean Implements IViewNoticeboardBlock.AllowPrint
        Get
            Return HYPprintNoticeboard.Visible
        End Get
        Set(value As Boolean)
            HYPprintNoticeboard.Visible = value
        End Set
    End Property
    Private Property CurrentLayout As PlainLayout Implements IViewNoticeboardBlock.CurrentLayout
        Get
            Return ViewStateOrDefault("CurrentLayout", PlainLayout.ignore)
        End Get
        Set(value As PlainLayout)
            ViewState("CurrentLayout") = value
        End Set
    End Property
#End Region
#Region "Internal"
    Public Property IsPreview As Boolean
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
            HYPmanageNoticeboard.Enabled = Not value
            HYPprintNoticeboard.Enabled = Not value
        End Set
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTnoticeboardTitle)
            .setHyperLink(HYPmanageNoticeboard, False, True)
            .setHyperLink(HYPprintNoticeboard, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControl(layout As PlainLayout, idCommunity As Integer) Implements IViewNoticeboardBlock.InitalizeControl
        CurrentLayout = layout
        CurrentPresenter.InitView(idCommunity)
    End Sub
    Private Sub DisplayEdit(url As String) Implements IViewNoticeboardBlock.DisplayEdit
        If String.IsNullOrEmpty(url) Then
            HYPmanageNoticeboard.Visible = False
        Else
            HYPmanageNoticeboard.Visible = True
            Me.HYPmanageNoticeboard.NavigateUrl = BaseUrl & url
        End If
    End Sub
    Private Sub LoadMessage(idMessage As Long, idCommunity As Integer) Implements IViewNoticeboardBlock.LoadMessage
        HYPnoticeboard.Visible = True

        HYPnoticeboard.Text = String.Format(LTexpandNoticeboardTemplate.Text, Resource.getValue("Noticeboard.expand"))
        HYPnoticeboard.NavigateUrl = BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.ModalMessagePage(idMessage, idCommunity)
        LTrenderNoticeboard.Text = String.Format(LTrenderNoticeboardTemplate.Text, BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessage(idMessage, idCommunity))
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewNoticeboardBlock.DisplaySessionTimeout
        HYPnoticeboard.Visible = False
    End Sub

#End Region

    Public Function GetItemColspan() As Integer
        Select Case CurrentLayout
            Case PlainLayout.box7box5
                Return 5
            Case PlainLayout.box8box4
                Return 4
            Case PlainLayout.full
                Return 0
            Case PlainLayout.ignore
                Return 0
        End Select
    End Function
  
   
End Class