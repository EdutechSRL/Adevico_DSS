Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_StatusLegend
    Inherits BaseUserControl

#Region "Internal"
    'Private Property ForActivities As Boolean
    '    Get
    '        ViewStateOrDefault("ForActivities", True)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("ForActivities") = value
    '    End Set
    'End Property
    Private Property OnlyAvailableStatus As Boolean
        Get
            ViewStateOrDefault("OnlyAvailableStatus", True)
        End Get
        Set(value As Boolean)
            ViewState("OnlyAvailableStatus") = value
        End Set
    End Property

    Private Property AvaialbeStatus As List(Of FieldStatus)
        Get
            Return ViewStateOrDefault("AvaialbeStatus", New List(Of FieldStatus) From {FieldStatus.completed, FieldStatus.error, FieldStatus.notstarted, FieldStatus.recalc, FieldStatus.started, FieldStatus.updated})
        End Get
        Set(value As List(Of FieldStatus))
            ViewState("AvaialbeStatus") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBlegendLabel)
            .setLabel(LBlegendtextcompleted)
            .setLabel(LBlegendtextstarted)
            .setLabel(LBlegendtextnotstarted)
            .setLabel(LBlegendtextrecalc)
            .setLabel(LBlegendtextupdated)
            .setLabel(LBlegendtexterror)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(aStatus As List(Of FieldStatus))
        AvaialbeStatus = aStatus
    End Sub
    Protected Function GetStatusToolTip(status As FieldStatus) As String
        'Return Resource.getValue("ForActivities." & ForActivities.ToString & ".GetStatusToolTip." & status.ToString)
        Return Resource.getValue("GetStatusToolTip." & status.ToString)
    End Function
    Protected Function IsAvailable(status As FieldStatus) As String
        Return IIf(Not OnlyAvailableStatus OrElse IsNothing(AvaialbeStatus) OrElse AvaialbeStatus.Contains(status), "", " hidden")
    End Function
#End Region
End Class