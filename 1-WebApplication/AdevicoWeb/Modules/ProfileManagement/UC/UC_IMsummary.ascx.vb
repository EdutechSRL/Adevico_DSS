Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_IMsummary
    Inherits BaseControl
    Implements IViewProfilesImportSummary

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isCompleted As Boolean Implements IViewProfilesImportSummary.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewProfilesImportSummary.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
            .setLabel(LBsummaryAuthenticationType_t)
            .setLabel(LBsummaryUserType_t)
            .setLabel(LBsummaryPrimaryOrganization_t)
            .setLabel(LBsummaryOtherOrganizations_t)
            .setLabel(LBsummaryMailSelector_t)
            .setLabel(LBsummarySubscriptions_t)
            .setLabel(LBsummarySubscribeTo_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewProfilesImportSummary.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub

    Public Sub InitializeControl(authenticationType As String, profileType As String, sendMailToUsers As Boolean, primaryOrganizationName As String, otherOrganizationsToSubscribe As Dictionary(Of Integer, String), subscriptions As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoNewProfileSubscription), profileToCreate As Integer) Implements IViewProfilesImportSummary.InitializeControl
        If CurrentContext.UserContext.isAnonymous Then
            DisplaySessionTimeout()
        Else
            Me.LBauthenticationType.Text = authenticationType
            Me.LBuserType.Text = profileType
            Resource.setLabel_To_Value(LBsummaryMailSelector, "LBsummaryMailSelector." & sendMailToUsers.ToString())
            Me.LBsummaryPrimaryOrganization.Text = primaryOrganizationName

            Dim info As String = ""
            Me.DVotherOrganizations.Visible = (otherOrganizationsToSubscribe.Count > 0)
            Me.LBsummaryOtherOrganizations.Text = ""
            If otherOrganizationsToSubscribe.Count > 0 Then
                For Each o As KeyValuePair(Of Int32, String) In otherOrganizationsToSubscribe
                    info &= "<li>" & Me.LBsummaryOtherOrganizations.Text & o.Value & "</li>"
                Next
                Me.LBsummaryOtherOrganizations.Text = "<ul>" & info & "</ul>"
            End If
            Me.LBsummarySubscriptions.Text = profileToCreate.ToString

            subscriptions = subscriptions.Where(Function(s) s.Node.PathDepth > 0).ToList()
            info = ""
            Dim translation As String = Resource.getValue("summarySubscription")
            For Each s As lm.Comol.Core.BaseModules.CommunityManagement.dtoNewProfileSubscription In subscriptions
                info = info & "<li>" & String.Format(translation, s.Name, s.RoleName) & "</li>"
            Next
            If String.IsNullOrEmpty(info) Then
                Me.LBsummarySubscribeTo.Text = ""
            Else
                Me.LBsummarySubscribeTo.Text = "<ul>" & info & "</ul>"
            End If
            Me.DVsubscribeTo.Visible = (subscriptions.Count > 0)
            Me.MLVcontrolData.SetActiveView(VIWsummary)
        End If

    End Sub

#End Region
End Class