Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports System.Linq
Public Class UC_ActivityInLineAdd
    Inherits BaseUserControl

#Region "Internal"
    Public Event AddActivity()
    Public Event AddActivities(ByVal number As Integer, ByVal isLinked As Boolean)
    Public Event AddSummaryActivity(ByVal number As Integer, ByVal isLinked As Boolean, ByVal children As Integer)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBaddActivityToMap, False, True)

            .setLabel(LBgroupAddActivities)
            LBgroupAddActivities.Text = String.Format(LBgroupAddActivities.Text, LTaddGroupTitle.Text)
            .setLabel(LBgroupAddLinkedActivities)
            LBgroupAddLinkedActivities.Text = String.Format(LBgroupAddLinkedActivities.Text, LTaddGroupTitle.Text)
            .setLabel(LBgroupAddSummary)
            LBgroupAddSummary.Text = String.Format(LBgroupAddSummary.Text, LTaddGroupTitle.Text)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(ByVal addActivity As Boolean, addLinked As Boolean, addSummary As Boolean)

        LTaddActivityToMap.Visible = addActivity
        LNBaddActivityToMap.Visible = addActivity

        LBgroupAddSummary.Visible = addSummary
        SPNaddSummary.Visible = addSummary

        LTaddActivitiesSeparator.Visible = addActivity
        LBgroupAddActivities.Visible = addActivity
        SPNaddActivities.Visible = addActivity

        LTaddLinkedActivitiesSeparator.Visible = addLinked
        LBgroupAddLinkedActivities.Visible = addLinked
        SPNaddLinkedActivities.Visible = addLinked
    End Sub

    Protected Sub AddActivity_Click(sender As Object, e As System.EventArgs)
        Dim number As Integer = CInt(sender.CommandArgument)
        Select Case sender.CommandName
            Case "addsummary"
                RaiseEvent AddSummaryActivity(1, False, number)
            Case "addlinked"
                RaiseEvent AddActivities(number, True)
            Case "addactivities"
                RaiseEvent AddActivities(number, False)
            Case Else
                RaiseEvent AddActivity()
        End Select
    End Sub
End Class