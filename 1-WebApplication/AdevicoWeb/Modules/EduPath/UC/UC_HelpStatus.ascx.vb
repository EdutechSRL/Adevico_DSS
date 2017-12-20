Imports lm.Comol.Modules.EduPath.Domain.StatusStatistic
Imports lm.Comol.Modules.EduPath.BusinessLogic.RootObject


Public Class UC_HelpStatus
    Inherits BaseControlSession


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpView", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub Init()
        With Me.Resource
            .setLabel(Me.LBStatusTitle)
            .setLabel(Me.LBStatusImg)

            Me.LBcompletedPassed.Text = .getValue("StatusStatistic." & CompletedPassed.ToString)
            Me.IMGcompletedPassed.ImageUrl = ImgStatusGreenMedium(Me.BaseUrl)

            Me.LBstarted.Text = .getValue("StatusStatistic." & Started.ToString)
            Me.IMGstarted.ImageUrl = ImgStatusYellowMedium(Me.BaseUrl)

            Me.LBbrowsed.Text = .getValue("StatusStatistic." & Browsed.ToString)
            Me.IMGbrowsed.ImageUrl = ImgStatusGreyMedium(Me.BaseUrl)

        End With
    End Sub

End Class