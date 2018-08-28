Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class EditDashboardViews
    Inherits DBbaseEditSettings
    Implements IViewEditViews

#Region "Context"
    Private _presenter As EditViewsPresenter
    Protected Friend ReadOnly Property CurrentPresenter As EditViewsPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New EditViewsPresenter(PageUtility.CurrentContext, Me)
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
        CurrentPresenter.InitView(PreloadIdDashboard, PreloadDashboardType, PreloadIdCommunity, True)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.EditViews")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.EditViews.NoPermission")
            .setHyperLink(HYPbackToDashboardListBottom, False, True)
            .setHyperLink(HYPbackToDashboardListTop, False, True)
            .setButton(BTNsaveDashboardViewsTop, True)
            .setButton(BTNsaveDashboardViewsBottom, True)
            .setHyperLink(HYPpreviewDashboardTop, False, True)
            .setHyperLink(HYPpreviewDashboardBottom, False, True)
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.DashboardEditViews(PreloadIdDashboard, PreloadDashboardType), IdContainerCommunity)
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
        Return IIf(top, BTNsaveDashboardViewsTop, BTNsaveDashboardViewsBottom)
    End Function
    Protected Friend Overrides Function GetStepsControl() As UC_GenericWizardSteps
        Return CTRLsteps
    End Function
#End Region

#Region "Implements"
    Private Sub LoadSettings(settings As dtoViewSettings, initialize As Boolean) Implements IViewEditViews.LoadSettings
        CTRLsettings.Visible = True
        CTRLsettings.InitializeControl(settings, initialize)
    End Sub
    Private Sub DisplayNoPageError() Implements IViewEditViews.DisplayNoPageError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoPageError"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayTileUnableToRedirectOn() Implements IViewEditViews.DisplayTileUnableToRedirectOn
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayTileUnableToRedirectOn"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayRangeError(settings As dtoViewSettings, pages As List(Of dtoPageSettings)) Implements IViewEditViews.DisplayRangeError
        CTRLmessages.Visible = True
        Dim key As String = "DisplayRangeError."
        Dim message As String = ""
        If pages.Count > 1 Then
            key &= "n"
        Else
            key &= "1"
        End If
        message = String.Format(Resource.getValue(key), String.Join(",", pages.Select(Function(p) Resource.getValue("DisplayRangeError.DashboardViewType." & p.Type.ToString)).ToArray()))
        CTRLmessages.InitializeControl(message, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
#End Region

#Region "Internal"
    Private Sub DashboardList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Private Sub BTNsaveDashboardViewsTop_Click(sender As Object, e As EventArgs) Handles BTNsaveDashboardViewsTop.Click, BTNsaveDashboardViewsBottom.Click
        CurrentPresenter.SaveViews(IdDashboard, CTRLsettings.GetSettings())
    End Sub
#End Region

End Class