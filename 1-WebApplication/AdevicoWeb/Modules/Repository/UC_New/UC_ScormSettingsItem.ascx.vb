Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Public Class UC_ScormSettingsItem
    Inherits FRbaseControl

#Region "Internal"
    Public Property ReadOnlyMode As Boolean
        Get
            Return ViewStateOrDefault("ReadOnlyMode", False)
        End Get
        Set(value As Boolean)
            ViewState("ReadOnlyMode") = value
        End Set
    End Property
    Public ReadOnly Property CssClass As String
        Get
            If (ReadOnlyMode) Then
                Return " readonly"
            Else
                Return ""
            End If
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBcheckScormCompletion)
            .setLabel(LBcheckTime)
            .setLabel(LBcheckScore)
            .setRangeValidator(RNVscore)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(package As lm.Comol.Core.FileRepository.Domain.ScormSettings.dtoScormPackageSettings, isReadOnly As Boolean)
        SetInternazionalizzazione()
        ReadOnlyMode = isReadOnly
        CBXscoreCompletion.Disabled = isReadOnly
        CBXtimeCompletion.Disabled = isReadOnly
        CBXscormCompletion.Disabled = isReadOnly
        INscore.Disabled = isReadOnly
        'INhours.Disabled = isReadOnly
        'INminutes.Disabled = isReadOnly
        'INseconds.Disabled = isReadOnly
        CBXscoreCompletion.Checked = package.CheckScore
        If package.CheckScore Then
            INscore.Value = package.MinScore.ToString
        End If
        CBXscormCompletion.Checked = package.CheckScormCompletion
        CBXtimeCompletion.Checked = package.CheckTime
        If package.CheckTime AndAlso package.MinTime > 0 Then
            Dim timespan As New TimeSpan(0, 0, package.MinTime)
            INhours.Value = timespan.Hours
            INminutes.Value = timespan.Minutes
            INseconds.Value = timespan.Seconds
        Else
            INhours.Value = 0
            INminutes.Value = 0
            INseconds.Value = 0
        End If
        LTidItem.Text = package.Id
        LTforPackage.Text = True
    End Sub
    Public Sub InitializeControl(activity As lm.Comol.Core.FileRepository.Domain.ScormSettings.dtoScormActivitySettings, isReadOnly As Boolean)
        SetInternazionalizzazione()
        ReadOnlyMode = isReadOnly
        CBXscoreCompletion.Disabled = isReadOnly
        CBXtimeCompletion.Disabled = isReadOnly
        CBXscormCompletion.Disabled = isReadOnly
        INscore.Disabled = isReadOnly
        'INhours.Disabled = isReadOnly
        'INminutes.Disabled = isReadOnly
        'INseconds.Disabled = isReadOnly

        CBXscoreCompletion.Checked = activity.CheckScore
        If activity.CheckScore Then
            INscore.Value = activity.MinScore.ToString
        End If
        CBXscormCompletion.Checked = activity.CheckScormCompletion
        CBXtimeCompletion.Checked = activity.CheckTime
        If activity.CheckTime AndAlso activity.MinTime > 0 Then
            Dim timespan As New TimeSpan(0, 0, activity.MinTime)
            INhours.Value = timespan.Hours
            INminutes.Value = timespan.Minutes
            INseconds.Value = timespan.Seconds
        Else
            INhours.Value = 0
            INminutes.Value = 0
            INseconds.Value = 0
        End If
        LTidItem.Text = activity.Id
        LTforPackage.Text = False
    End Sub

    Public Function GetSettings() As lm.Comol.Core.FileRepository.Domain.ScormSettings.dtoScormItemEvaluationSettings
        Dim settings As New lm.Comol.Core.FileRepository.Domain.ScormSettings.dtoScormItemEvaluationSettings

        Long.TryParse(LTidItem.Text, settings.Id)
        Boolean.TryParse(LTforPackage.Text, settings.ForPackage)
        Dim hasTimeError As Boolean = False, hasScoreError As Boolean = False
        settings.CheckScore = CBXscoreCompletion.Checked
        If settings.CheckScore Then
            If Not Decimal.TryParse(INscore.Value, settings.MinScore) Then
                hasScoreError = True
                If Not INscore.Attributes("class").Contains("input-validation-error") Then
                    INscore.Attributes("class") &= " input-validation-error"
                End If
            Else
                INscore.Attributes("class") = Replace(INscore.Attributes("class"), " input-validation-error", "")
            End If
            INscore.Value = settings.MinScore
            settings.UseScoreScaled = False
        End If
        settings.CheckScormCompletion = CBXscormCompletion.Checked
        settings.CheckTime = CBXtimeCompletion.Checked
        If settings.CheckTime Then
            Dim hours As Integer = 0, minutes As Integer = 0, seconds As Integer = 0
            If Not Integer.TryParse(INhours.Value, hours) Then
                hasTimeError = True
                If Not INhours.Attributes("class").Contains("input-validation-error") Then
                    INhours.Attributes("class") &= " input-validation-error"
                End If
            Else
                INhours.Attributes("class") = Replace(INhours.Attributes("class"), " input-validation-error", "")
            End If
            If Not Integer.TryParse(INminutes.Value, minutes) Then
                hasTimeError = True
                If Not INminutes.Attributes("class").Contains("input-validation-error") Then
                    INminutes.Attributes("class") &= " input-validation-error"
                End If
            Else
                INminutes.Attributes("class") = Replace(INminutes.Attributes("class"), " input-validation-error", "")
            End If
            If Not Integer.TryParse(INseconds.Value, seconds) Then
                hasTimeError = True
                If Not INseconds.Attributes("class").Contains("input-validation-error") Then
                    INseconds.Attributes("class") &= " input-validation-error"
                End If
            Else
                INseconds.Attributes("class") = Replace(INseconds.Attributes("class"), " input-validation-error", "")
            End If

            Dim timespan As New TimeSpan(hours, minutes, seconds)
            settings.MinTime = timespan.TotalSeconds
        Else
            settings.MinTime = 0
        End If
        If hasTimeError AndAlso hasScoreError Then
            settings.Error = lm.Comol.Core.FileRepository.Domain.ScormSettings.ScormSettingsError.timescore
        ElseIf hasTimeError Then
            settings.Error = lm.Comol.Core.FileRepository.Domain.ScormSettings.ScormSettingsError.time
        ElseIf hasScoreError Then
            settings.Error = lm.Comol.Core.FileRepository.Domain.ScormSettings.ScormSettingsError.score
        End If
        Return settings
    End Function
#End Region
End Class