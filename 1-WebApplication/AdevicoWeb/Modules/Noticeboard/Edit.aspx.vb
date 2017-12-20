Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public Class EditNoticeboardMessage
    Inherits NBnoticeboardEditingPage
    Implements IViewEditMessage

#Region "Context"
    Private _Presenter As EditMessagePresenter
    Public ReadOnly Property CurrentPresenter() As EditMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditMessagePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadedIdMessage, True, PreloadedIdCommunity, PreloadFromPortal)
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPbackFromEditor, False, True)
            .setLinkButton(LNBsaveMessage, False, True)
            .setLinkButton(LNBsaveMessageAndExit, False, True)
            HYPbackFromEditorNoPermissions.Text = HYPbackFromEditor.Text
            HYPbackFromEditorNoPermissions.ToolTip = HYPbackFromEditor.ToolTip
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
        HYPbackFromEditor.NavigateUrl = PageUtility.ApplicationUrlBase & url
        HYPbackFromEditorNoPermissions.NavigateUrl = PageUtility.ApplicationUrlBase & url
        HYPbackFromEditor.Visible = True
        HYPbackFromEditorNoPermissions.Visible = True
    End Sub
    Protected Friend Overrides Sub SetHeaderTitle(isForPortal As Boolean, name As String)
        Dim message As String = ""
        If Not (IdCurrentMessage > 0) Then
            message = ".0"
        End If
        If isForPortal Then
            Master.ServiceTitle = Resource.getValue("EditMessageTitle.True" & message)
        ElseIf Not String.IsNullOrEmpty(name) AndAlso IdCurrentCommunity <> ComunitaCorrenteID Then
            Master.ServiceTitle = Resource.getValue("EditMessageTitle.False" & message)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("EditMessageTitle.False" & message & ".ToolTip"), name)
        ElseIf Not String.IsNullOrEmpty(name) Then
            Master.ServiceTitle = Resource.getValue("EditMessageTitle.False" & message)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("EditMessageTitle.False" & message & ".ToolTip"), name)
        Else
            Master.ServiceTitle = Resource.getValue("EditMessageTitle.False")
        End If
    End Sub
    Protected Friend Overrides Function GetControlMessages() As UC_ActionMessages
        Return CTRLmessages
    End Function
#End Region

    Private Sub AllowSave(allow As Boolean) Implements IViewEditMessage.AllowSave
        LNBsaveMessage.Visible = allow
        LNBsaveMessageAndExit.Visible = allow
    End Sub
    Private Sub DisplayUnknownMessage() Implements IViewEditMessage.DisplayUnknownMessage
        MLVcontent.SetActiveView(VIWnoPermissions)
        LTmessage.Text = Resource.getValue("IViewEditMessage.DisplayUnknownMessage")
    End Sub

    Private Sub EditMessage(message As NoticeboardMessage, advancedEditor As Boolean) Implements IViewEditMessage.EditMessage
        If advancedEditor <> message.CreateByAdvancedEditor Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(Resource.getValue("EditMessage.Simple"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        End If
        CTRLeditor.HTML = message.Message
    End Sub

#End Region

#Region "internal"
    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Master.BRheaderActive = False
        'Master.DisplayTitleRow = False
        Master.ShowDocType = True
        'Master.ShowHeaderLanguageChanger = True
    End Sub
    Private Sub LNBsaveMessage_Click(sender As Object, e As EventArgs) Handles LNBsaveMessage.Click
        SaveMessage()
    End Sub
    Private Sub LNBsaveMessageAndExit_Click(sender As Object, e As EventArgs) Handles LNBsaveMessageAndExit.Click
        SaveMessage(True)
    End Sub
    Private Sub SaveMessage(Optional gotoManagement As Boolean = False)
        CTRLmessages.Visible = False
        CurrentPresenter.SaveMessage(gotoManagement, IdCurrentMessage, True, IdCurrentCommunity, IsPortalPage, NoticeboardFilePath & IdCurrentCommunity.ToString & "\", PageUtility.ApplicationUrlBase, OLDpageUtility.ApplicationWorkingId, PageUtility.ApplicationUrlBase(True), CTRLeditor.HTML, CTRLeditor.Text)
    End Sub
#End Region
  
End Class