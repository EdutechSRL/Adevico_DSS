Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public Class NoticeboardDashboard
    Inherits NBnoticeboardEditingPage
    Implements IViewNoticeboardDashboard

#Region "Context"
    Private _Presenter As NoticeboardDashboardPresenter
    Public ReadOnly Property CurrentPresenter() As NoticeboardDashboardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NoticeboardDashboardPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadedIdCommunity, PreloadFromPortal, PreloadBackUrl, PreloadedIdMessage)
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPeditWihAdvancedEditor, False, True)
            .setHyperLink(HYPeditWihSimpleEditor, False, True)
            .setHyperLink(HYPnewWihAdvancedEditor, False, True)
            .setHyperLink(HYPnewWihSimpleEditor, False, True)
            .setLinkButton(LNBaddEmptyMessage, False, True)
            .setLinkButton(LNBvirtualDeleteMessage, False, True)
            .setLinkButton(LNBvirtualUndeleteMessage, False, True)
            .setLinkButton(LNBvirtualUndeleteAndActivate, False, True)

            .setLinkButton(LNBsetActive, False, True)
            .setHyperLink(HYPprintNoticeboard, False, True)
            .setHyperLink(HYPbackToPreviousPage, False, True)
            .setHyperLink(HYPbackToPreviousPageNoPermissions, False, True)
        End With
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVcontent.SetActiveView(VIWnoPermissions)
        LTmessage.Text = Resource.getValue("NoticeboardRenderPage.NoPermessi")
    End Sub
#End Region


#Region "Implements"
#Region "Inherits"
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.NoticeboardDashboard(IdCurrentMessage, IdCurrentCommunity, IsPortalPage, False), IdCurrentCommunity)
    End Sub
    Protected Friend Overrides Sub SetBackUrl(url As String)
        HYPbackToPreviousPage.NavigateUrl = PageUtility.ApplicationUrlBase & url
        HYPbackToPreviousPageNoPermissions.NavigateUrl = PageUtility.ApplicationUrlBase & url
        HYPbackToPreviousPage.Visible = True
        HYPbackToPreviousPageNoPermissions.Visible = True
        If IsPortalPage Then
            HYPbackToPreviousPage.Text = Resource.getValue("HYPbackToPreviousPage.Text.IsPortal")
            HYPbackToPreviousPage.ToolTip = Resource.getValue("HYPbackToPreviousPage.ToolTip.IsPortal")
        Else
            HYPbackToPreviousPage.Text = Resource.getValue("HYPbackToPreviousPage.Text.IsCommunity")
            HYPbackToPreviousPage.ToolTip = Resource.getValue("HYPbackToPreviousPage.ToolTip.IsCommunity")
        End If
        HYPbackToPreviousPageNoPermissions.Text = HYPbackToPreviousPage.Text
        HYPbackToPreviousPageNoPermissions.ToolTip = HYPbackToPreviousPage.ToolTip
    End Sub
    Protected Friend Overrides Sub SetHeaderTitle(isForPortal As Boolean, name As String)
        If isForPortal Then
            Master.ServiceTitle = Resource.getValue("SetHeaderTitle.True")
        ElseIf Not String.IsNullOrEmpty(name) AndAlso IdCurrentCommunity <> ComunitaCorrenteID Then
            Master.ServiceTitle = String.Format(Resource.getValue("SetHeaderTitle"), name)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("SetHeaderTitle.Tooltip"), name)
        ElseIf Not String.IsNullOrEmpty(name) Then
            Master.ServiceTitle = Resource.getValue("SetHeaderTitle.False")
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("SetHeaderTitle.Tooltip"), name)
        Else
            Master.ServiceTitle = Resource.getValue("SetHeaderTitle.False")
        End If
    End Sub
    Protected Friend Overrides Function GetControlMessages() As UC_ActionMessages
        Return CTRLmessages
    End Function
#End Region
    Private Function GetBackUrl() As String Implements IViewNoticeboardDashboard.GetBackUrl
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            url = Request.UrlReferrer.PathAndQuery
            If url.StartsWith(PageUtility.BaseUrl) AndAlso Not PageUtility.BaseUrl = "/" Then
                url = url.Replace(PageUtility.BaseUrl, "")
            ElseIf url.StartsWith(PageUtility.BaseUrl) AndAlso PageUtility.BaseUrl = "/" Then
                url = url.Replace("//", "/")
            End If
        End If
        Return url
    End Function
    Private Function GetDefaultHomePage() As String Implements IViewNoticeboardDashboard.GetDefaultHomePage
        Return SystemSettings.Presenter.DefaultLogonPage
    End Function
    Private Sub AllowVirtualDelete(allow As Boolean) Implements IViewNoticeboardDashboard.AllowVirtualDelete
        LNBvirtualDeleteMessage.Visible = allow
    End Sub
    Private Sub AllowVirtualUndelete(allow As Boolean) Implements IViewNoticeboardDashboard.AllowVirtualUndelete
        DVvirtualUndeleteButtons.Visible = allow
    End Sub
    Private Sub AllowVirtualUndeleteAndSetActive(allow As Boolean) Implements IViewNoticeboardDashboard.AllowVirtualUndeleteAndSetActive
        LNBvirtualUndeleteAndActivate.Visible = allow
    End Sub
    Private Sub AllowSetActive(allow As Boolean) Implements IViewNoticeboardDashboard.AllowSetActive
        LNBsetActive.Visible = allow
    End Sub
    Private Sub HideEditingCommands() Implements IViewNoticeboardDashboard.HideEditingCommands
        DVeditButtons.Visible = False
    End Sub
    Private Sub SetEditingUrls(urlAdvanced As String, urlSimple As String) Implements IViewNoticeboardDashboard.SetEditingUrls
        DVeditButtons.Visible = Not (String.IsNullOrEmpty(urlAdvanced) AndAlso String.IsNullOrEmpty(urlSimple))
        HYPeditWihAdvancedEditor.NavigateUrl = ApplicationUrlBase & urlAdvanced
        HYPeditWihSimpleEditor.NavigateUrl = ApplicationUrlBase & urlSimple
    End Sub
    Private Sub InitializeControl(idCommunity As Integer, permissions As ModuleNoticeboard, Optional idMessage As Long = 0) Implements IViewNoticeboardDashboard.InitializeControl
        CTRLrender.InitializeControl(idCommunity, permissions, idMessage)
    End Sub
    Private Sub SetNewMessageUrls(urlAdvanced As String, urlSimple As String, allowClean As Boolean) Implements IViewNoticeboardDashboard.SetNewMessageUrls
        DVaddButtons.Visible = True
        HYPnewWihAdvancedEditor.NavigateUrl = ApplicationUrlBase & urlAdvanced
        HYPnewWihSimpleEditor.NavigateUrl = ApplicationUrlBase & urlSimple
        LNBaddEmptyMessage.Visible = allowClean
    End Sub

#End Region

#Region "internal"
    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Master.BRheaderActive = False
        'Master.DisplayTitleRow = False
        Master.ShowDocType = True
        'Master.ShowHeaderLanguageChanger = True
    End Sub
    Private Sub CTRLrender_SelectedMessage(idMessage As Long) Handles CTRLrender.SelectedMessage
        CTRLmessages.Visible = False
        CurrentPresenter.SelectedMessage(IdCurrentCommunity, IsPortalPage, idMessage)
    End Sub

    Private Sub LNBsetActive_Click(sender As Object, e As EventArgs) Handles LNBsetActive.Click
        CTRLmessages.Visible = False
        CurrentPresenter.SetActive(IdCurrentMessage, IdCurrentCommunity, IsPortalPage, NoticeboardFilePath & IdCurrentCommunity.ToString & "\", PageUtility.ApplicationUrlBase, OLDpageUtility.ApplicationWorkingId, PageUtility.ApplicationUrlBase(True))
    End Sub

    Private Sub LNBaddEmptyMessage_Click(sender As Object, e As EventArgs) Handles LNBaddEmptyMessage.Click
        CTRLmessages.Visible = False
        CurrentPresenter.ClearNoticeBoard(IdCurrentCommunity, IsPortalPage, NoticeboardFilePath & IdCurrentCommunity.ToString & "\", PageUtility.ApplicationUrlBase, OLDpageUtility.ApplicationWorkingId, PageUtility.ApplicationUrlBase(True))
    End Sub

    Private Sub LNBvirtualDeleteMessage_Click(sender As Object, e As EventArgs) Handles LNBvirtualDeleteMessage.Click
        CTRLmessages.Visible = False
        CurrentPresenter.VirtualDeleteMessage(IdCurrentMessage, IdCurrentCommunity, IsPortalPage, OLDpageUtility.ApplicationWorkingId)
    End Sub

    Private Sub LNBvirtualUndeleteAndActivate_Click(sender As Object, e As EventArgs) Handles LNBvirtualUndeleteAndActivate.Click
        CTRLmessages.Visible = False
        CurrentPresenter.VirtualUnDeleteMessage(IdCurrentMessage, True, IdCurrentCommunity, IsPortalPage, OLDpageUtility.ApplicationWorkingId, NoticeboardFilePath & IdCurrentCommunity.ToString & "\", PageUtility.ApplicationUrlBase, PageUtility.ApplicationUrlBase(True))
    End Sub

    Private Sub LNBvirtualUndeleteMessage_Click(sender As Object, e As EventArgs) Handles LNBvirtualUndeleteMessage.Click
        CTRLmessages.Visible = False
        CurrentPresenter.VirtualUnDeleteMessage(IdCurrentMessage, False, IdCurrentCommunity, IsPortalPage, OLDpageUtility.ApplicationWorkingId)
    End Sub
#End Region


    Public Property CurrentPermissions As ModuleNoticeboard Implements IViewNoticeboardDashboard.CurrentPermissions

End Class