Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_ConfirmProjectSettings
    Inherits BaseControl
    Implements IViewConfirmSettingsToApply

#Region "Context"
    Private _Presenter As ConfirmSettingsToApplyPresenter
    Private ReadOnly Property CurrentPresenter() As ConfirmSettingsToApplyPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ConfirmSettingsToApplyPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewConfirmSettingsToApply.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayApply As Boolean Implements IViewConfirmSettingsToApply.DisplayApply
        Get
            Return ViewStateOrDefault("DisplayApply", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayApply") = value
            BTNapplySettingsActions.Visible = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewConfirmSettingsToApply.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewConfirmSettingsToApply.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewConfirmSettingsToApply.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
#End Region

#End Region

#Region "Property"
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property
    Public Event CloseWindow()
    Public Event ApplyActions(actions As dtoProjectSettingsSelectedActions)
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
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNapplySettingsActions, True)
            .setButton(BTNcancelSettingsActions, True)

            .setLabel(LBdateactionsInfo_t)
            .setLabel(LBcpmactionsInfo_t)
            .setLabel(LBmanualactionsInfo_t)
            .setLabel(LBmilestonesactionsInfo_t)
            .setLabel(LBsummariesactionsInfo_t)
            .setLabel(LBestimatedactionsInfo_t)

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProject As Long, dto As dtoProject, cStatistic As dtoProjectStatistics, Optional description As String = "") Implements IViewConfirmSettingsToApply.InitializeControl
        SetInternazionalizzazione()
        Me.CurrentPresenter.InitView(idProject, dto, cStatistic, description)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewConfirmSettingsToApply.DisplaySessionTimeout
        DVdescription.Visible = True
        LBdescription.Text = Resource.getValue("IViewConfirmSettingsToApply.DisplaySessionTimeout")
    End Sub
    Private Sub DisplayNoPermissionToApply() Implements IViewConfirmSettingsToApply.DisplayNoPermissionToApply
        DVdescription.Visible = True
        LBdescription.Text = Resource.getValue("IViewConfirmSettingsToApply.DisplayNoPermissionToApply")
    End Sub
    Private Sub SetDescription(description As String) Implements IViewConfirmSettingsToApply.SetDescription
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LBdescription.Text = description
        End If
    End Sub

    Private Sub LoadActions(actions As dtoProjectSettingsAction, Optional ByVal currentDate As DateTime? = Nothing, Optional ByVal newDate As DateTime? = Nothing) Implements IViewConfirmSettingsToApply.LoadActions
        DVcpmactions.Visible = Not IsNothing(actions.CpmActions)
        DVmanualactions.Visible = Not IsNothing(actions.ManualActions)
        DVestimatedactions.Visible = Not IsNothing(actions.EstimatedActions)
        DVmilestonesactions.Visible = Not IsNothing(actions.MilestonesActions)
        DVsummariesactions.Visible = Not IsNothing(actions.SummariesActions)
        DVdateactions.Visible = Not IsNothing(actions.DateActions)
        If Not IsNothing(actions.CpmActions) Then
            LoadActions("cpmactions", actions.CpmActions, actions.Activities)
        ElseIf Not IsNothing(actions.ManualActions) Then
            LoadActions("manualactions", actions.ManualActions, actions.Activities)
        End If
        If Not IsNothing(actions.EstimatedActions) Then
            LoadActions("estimatedactions", actions.EstimatedActions, actions.EstimatedActivities)
        End If
        If Not IsNothing(actions.MilestonesActions) Then
            LoadActions("milestonesactions", actions.MilestonesActions, actions.Milestones)
        End If
        If Not IsNothing(actions.SummariesActions) Then
            LoadActions("summariesations", actions.SummariesActions, actions.Summaries)
        End If
        If Not IsNothing(actions.DateActions) Then
            LoadActions("dateactions", actions.DateActions, actions.Activities, currentDate, newDate)
        End If
    End Sub
    Private Sub LoadActions(name As String, actions As List(Of ConfirmActions), activities As Long, ByVal currentDate As DateTime?, ByVal newDate As DateTime?)
        LoadActions(name, actions)
        If Not String.IsNullOrEmpty(LBdateactionsInfo_t.Text) AndAlso LBdateactionsInfo_t.Text.Contains("{0}") AndAlso LBdateactionsInfo_t.Text.Contains("{1}") Then
            LBdateactionsInfo_t.Text = String.Format(LBdateactionsInfo_t.Text, FormatDateTime(currentDate.Value.ToShortDateString()), FormatDateTime(newDate.Value.ToShortDateString()))
        End If
    End Sub
    Private Sub LoadActions(name As String, actions As List(Of ConfirmActions), activities As Long)
        LoadActions(name, actions)
        Dim oLabel As Label = FindControl("LB" & name & "Info_t")
        If activities > 1 Then
            oLabel.Text = String.Format(Resource.getValue("LB" & name & "Info_t.n"), activities)
        End If
    End Sub
    Private Sub LoadActions(name As String, actions As List(Of ConfirmActions))
        Dim oRepeater As Repeater = FindControl("RPT" & name)
        oRepeater.DataSource = actions.Select(Function(a) New lm.Comol.Core.DomainModel.TranslatedItem(Of ConfirmActions) With {.Id = a, .Translation = "ConfirmActions." & name & "."}).ToList()
        oRepeater.DataBind()
    End Sub
    Private Function GetSelectedActions() As dtoProjectSettingsSelectedActions Implements IViewConfirmSettingsToApply.GetSelectedActions
        Dim result As New dtoProjectSettingsSelectedActions

        If DVcpmactions.Visible Then
            result.CpmAction = GetSelectedAction("RDcpmactions")
        ElseIf DVmanualactions.Visible Then
            result.ManualAction = GetSelectedAction("RDmanualactions")
        End If
        If DVdateactions.Visible Then
            result.DateAction = GetSelectedAction("RDdateactions")
        End If
        If DVestimatedactions.Visible Then
            result.EstimatedAction = GetSelectedAction("RDestimatedactions")
        End If
        If DVmilestonesactions.Visible Then
            result.MilestonesAction = GetSelectedAction("RDmilestonesactions")
        End If
        If DVsummariesactions.Visible Then
            result.SummariesAction = GetSelectedAction("RDsummariesactions")
        End If
        Return result
    End Function
    Private Function GetSelectedAction(name As String) As ConfirmActions
        Dim value As String = Request.Form(name)
        If Not String.IsNullOrEmpty(value) AndAlso IsNumeric(value) Then
            Return value
        Else
            Return ConfirmActions.None
        End If
        Return ConfirmActions.None
    End Function

#End Region

    Private Sub BTNcancelSettingsActions_Click(sender As Object, e As System.EventArgs) Handles BTNcancelSettingsActions.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
    Private Sub BTNapplySettingsActions_Click(sender As Object, e As System.EventArgs) Handles BTNapplySettingsActions.Click
        If RaiseCommandEvents Then
            RaiseEvent ApplyActions(GetSelectedActions())
        End If
    End Sub
    Protected Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim action As lm.Comol.Core.DomainModel.TranslatedItem(Of ConfirmActions) = DirectCast(e.Item.DataItem, lm.Comol.Core.DomainModel.TranslatedItem(Of ConfirmActions))

        Dim oLabel As Label =  e.Item.FindControl("LBactionDescription")
        oLabel.Text = Resource.getValue(action.Translation & action.Id.ToString & ".description")
    End Sub

    Protected Function SetChecked(action As lm.Comol.Core.DomainModel.TranslatedItem(Of ConfirmActions)) As String
        Return IIf(action.Id = ConfirmActions.Hold, "checked=""checked""", "")
    End Function
End Class