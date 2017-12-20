Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class EditBaseDashboardSettings
    Inherits DBbaseEditSettings
    Implements IViewEditSettings

#Region "Context"
    Private _presenter As EditSettingsPresenter
    Protected Friend ReadOnly Property CurrentPresenter As EditSettingsPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New EditSettingsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(False, PreloadIdDashboard, PreloadDashboardType, PreloadIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.EditSettings")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.EditSettings.NoPermission")
            .setHyperLink(HYPbackToDashboardListBottom, False, True)
            .setHyperLink(HYPbackToDashboardListTop, False, True)
            .setButton(BTNsaveDashboardSettingsTop, True)
            .setButton(BTNsaveDashboardSettingsBottom, True)
            .setHyperLink(HYPpreviewDashboardTop, False, True)
            .setHyperLink(HYPpreviewDashboardBottom, False, True)
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.DashboardEdit(PreloadIdDashboard, PreloadDashboardType), IdContainerCommunity)
    End Sub

    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return IIf(top, HYPbackToDashboardListTop, HYPbackToDashboardListBottom)
    End Function
    Protected Friend Overrides Function GetMessageControl() As UC_ActionMessages
        Return CTRLmessages
    End Function
    Protected Friend Overrides Function GetPreviewUrlItem(Optional top As Boolean = True) As HyperLink
        Return IIf(top, HYPpreviewDashboardTop, HYPpreviewDashboardBottom)
    End Function
    Protected Friend Overrides Function GetSaveButton(Optional top As Boolean = True) As Button
        Return IIf(top, BTNsaveDashboardSettingsTop, BTNsaveDashboardSettingsBottom)
    End Function
    Protected Friend Overrides Function GetStepsControl() As UC_GenericWizardSteps
        Return CTRLsteps
    End Function
#End Region

#Region "Implements"
    Private Sub DisplaySettingsAdded() Implements IViewEditSettings.DisplaySettingsAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySettingsAdded"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Private Function GetNewSettingsName() As String Implements IViewEditSettings.GetNewSettingsName
        Resource.getValue("GetNewSettingsName")
    End Function
    Private Sub EnableSelector(enable As Boolean) Implements IViewEditSettings.EnableSelector
        CTRLsettings.EnableSelector(enable)
    End Sub
#End Region

#Region "internal"
    Private Sub DashboardList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Private Sub LoadSettings(settings As liteDashboardSettings) Implements IViewEditSettings.LoadSettings
        CTRLsettings.Visible = True
        CTRLsettings.InitializeControl(settings)
    End Sub
    Private Sub LoadSettings(settings As liteDashboardSettings, items As List(Of lm.Comol.Core.DomainModel.dtoTranslatedProfileType), selected As List(Of Integer)) Implements IViewEditSettings.LoadSettings
        CTRLsettings.Visible = True
        CTRLsettings.InitializeControl(settings, items, selected)
    End Sub
    Private Sub LoadSettings(settings As liteDashboardSettings, items As List(Of lm.Comol.Core.DomainModel.dtoTranslatedRoleType), selected As List(Of Integer)) Implements IViewEditSettings.LoadSettings
        CTRLsettings.Visible = True
        CTRLsettings.InitializeControl(settings, items, selected)
    End Sub
    Private Function GetSettings() As dtoBaseDashboardSettings Implements IViewEditSettings.GetSettings
        Return CTRLsettings.GetSettings()
    End Function
    Private Sub BTNsaveDashboardSettingsBottom_Click(sender As Object, e As EventArgs) Handles BTNsaveDashboardSettingsBottom.Click, BTNsaveDashboardSettingsTop.Click
        CurrentPresenter.SaveSettings(CTRLsettings.GetSettings())
    End Sub
#End Region

End Class