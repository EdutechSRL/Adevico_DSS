
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public Class NoticeboardRenderPage
    Inherits NBnoticeboardPage
    Implements IViewNoticeboardRenderPage

#Region "Context"
    Private _Presenter As NoticeboardRenderPagePresenter
    Public ReadOnly Property CurrentPresenter() As NoticeboardRenderPagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NoticeboardRenderPagePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadedIdCommunity, PreloadedIdMessage)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVdisplay.SetActiveView(VIWnoAccess)
        LTsessionTimeout.Text = Resource.getValue("NoticeboardRenderPage.NoPermessi")
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        Me.MLVdisplay.SetActiveView(VIWnoAccess)
        LTsessionTimeout.Text = Resource.getValue("NoticeboardRenderPage.SessionTimeout")
    End Sub
    Private Sub InitializeControl(idCommunity As Integer, permissions As ModuleNoticeboard, Optional idMessage As Long = 0) Implements IViewNoticeboardRenderPage.InitializeControl
        Me.MLVdisplay.SetActiveView(VIWmessage)
        CTRLrender.InitializeControl(idCommunity, permissions, idMessage)
    End Sub
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region




End Class