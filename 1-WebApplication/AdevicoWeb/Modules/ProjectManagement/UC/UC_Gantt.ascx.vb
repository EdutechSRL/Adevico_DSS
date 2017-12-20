Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_Gantt
    Inherits BaseControl
    Implements IViewDisplayGantt

#Region "Context"
    Private _Presenter As DisplayGanttPresenter
    Private ReadOnly Property CurrentPresenter() As DisplayGanttPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DisplayGanttPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Property FormatDatePattern As String Implements IViewDisplayGantt.FormatDatePattern
        Get
            Return ViewStateOrDefault("FormatDatePattern", LoaderCultureInfo.DateTimeFormat.ShortDatePattern)
        End Get
        Set(value As String)
            ViewState("FormatDatePattern") = value
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

#Region "Internal"
    Protected Function GetErrorDrawingGantt() As String
        Return Resource.getValue("GetErrorDrawingGantt")
    End Function

    Protected Property LoaderCultureInfo As System.Globalization.CultureInfo
        Get
            Return ViewStateOrDefault("LoaderCultureInfo", Resource.CultureInfo)
        End Get
        Set(value As System.Globalization.CultureInfo)
            ViewState("LoaderCultureInfo") = value
        End Set
    End Property
    Public Property DisplayCommandsTop As Boolean
        Get
            Return ViewStateOrDefault("DisplayCommandsTop", False)
        End Get
        Set(value As Boolean)
            DVcommandsTop.Visible = value
            ViewState("DisplayCommandsTop") = value
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
        With MyBase.Resource
            .setLiteral(LTganttGoToStartDateBottom)
            .setLiteral(LTganttGoToStartDateTop)
            .setLiteral(LTganttGoToTodayBottom)
            .setLiteral(LTganttGoToTodayTop)
            .setLabel(LBlegendLabel)


            .setLabel(LBlegendLabel)
            .setLabel(LBganttCriticalLegend)
            .setLabel(LBganttActivityLegend)
            .setLabel(LBganttSummaryLegend)
            .setLabel(LBganttActivityLinksLegend)
            .setLabel(LBganttFinishStartLegend)
            .setLabel(LBganttFinishFinishLegend)
            .setLabel(LBganttStartFinishLegend)
            .setLabel(LBganttStartStartLegend)
            .setLabel(LBganttTodayLegend)
            .setLabel(LBganttDeadLineLegend)
            SPNcritical.Attributes("title") = .getValue("SPNcritical.title")
            SPNactivity.Attributes("title") = .getValue("SPNactivity.title")
            SPNsummary.Attributes("title") = .getValue("SPNsummary.title")
            SPNfinishStart.Attributes("title") = .getValue("SPNfinishStart.title")
            SPNfinishFinish.Attributes("title") = .getValue("SPNfinishFinish.title")
            SPNstartFinish.Attributes("title") = .getValue("SPNstartFinish.title")
            SPNstartStart.Attributes("title") = .getValue("SPNstartStart.title")
            SPNtoday.Attributes("title") = .getValue("SPNtoday.title")
            SPNdeadline.Attributes("title") = .getValue("SPNdeadline.title")
        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeControl(project As dtoProject, formatDateString As String) Implements IViewDisplayGantt.InitializeControl
        FormatDatePattern = formatDateString
        CurrentPresenter.InitView(project, GetDatePatternEncoded())
    End Sub
    Public Sub InitializeControl(idProject As Long, formatDateString As String) Implements IViewDisplayGantt.InitializeControl
        FormatDatePattern = formatDateString
        CurrentPresenter.InitView(idProject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        MLVgantt.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        MLVgantt.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayUnknownProject() Implements IViewDisplayGantt.DisplayUnknownProject
        MLVgantt.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayNoPermissionToSeeProjectGantt() Implements IViewDisplayGantt.DisplayNoPermissionToSeeProjectGantt
        MLVgantt.SetActiveView(VIWempty)
    End Sub
    Private Function GetDatePatternEncoded() As String Implements IViewDisplayGantt.GetDatePatternEncoded
        Return EncodeItem(FormatDatePattern)
    End Function
    Private Sub LoadGantt(url As String) Implements IViewDisplayGantt.LoadGantt
        LTganttScript.Text = Replace(LTganttScriptTemplate.Text, "#url#", url)
        LTganttScript.Text = Replace(LTganttScript.Text, "#error#", GetErrorDrawingGantt())
        Dim oLiteral As Literal = FindControl("LTganttLanguage_" & Resource.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
        If Not IsNothing(oLiteral) Then
            If oLiteral.Text.Contains("{0}") Then
                oLiteral.Text = String.Format(oLiteral.Text, PageUtility.ApplicationUrlBase)
            End If
            oLiteral.Visible = True
        End If
    End Sub
#End Region

    Private Function EncodeItem(item As String) As String
        If String.IsNullOrEmpty(item) Then
            Return item
        Else
            Return PageUtility.GetUrlEncoded(item)
        End If
    End Function

End Class