Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class AutoLogon
    Inherits DBpageBase
    Implements IViewAutoLogonCommunity

#Region "Context"
    Private _Presenter As AutoLogonCommunityPresenter
    Private ReadOnly Property CurrentPresenter() As AutoLogonCommunityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AutoLogonCommunityPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(PreloadIdCommunity)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        PageUtility.RedirectToDefaultLogoutPage(PageUtility.GetDefaultLogoutPage())
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPgoToHomePageFromAutoLogonIntoCommunity, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayCommunityBlock(name As String) Implements IViewAutoLogonCommunity.DisplayCommunityBlock
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewAutoLogonCommunity.DisplayCommunityBlock"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNoAccessToCommunity(name As String) Implements IViewAutoLogonCommunity.DisplayNoAccessToCommunity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewAutoLogonCommunity.DisplayNoAccessToCommunity"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNotEnrolledIntoCommunity(name As String) Implements IViewAutoLogonCommunity.DisplayNotEnrolledIntoCommunity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewAutoLogonCommunity.DisplayNotEnrolledIntoCommunity"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnknownCommunity() Implements IViewAutoLogonCommunity.DisplayUnknownCommunity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewAutoLogonCommunity.DisplayUnknownCommunity"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWaitingActivaction(name As String) Implements IViewAutoLogonCommunity.DisplayWaitingActivaction
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewAutoLogonCommunity.DisplayWaitingActivaction"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub LogonToCommunity(idCommunity As Integer, idUser As Integer) Implements IViewAutoLogonCommunity.LogonToCommunity
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Session("LinguaCode"))

        PageUtility.AccessToCommunity(idUser, idCommunity, oResourceConfig, True)
    End Sub
    Private Sub RedirectToPortalHomePage() Implements IViewAutoLogonCommunity.RedirectToPortalHomePage
        PageUtility.RedirectToUrl(SystemSettings.Presenter.DefaultLogonPage)
    End Sub
#End Region

End Class