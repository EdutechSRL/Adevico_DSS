Imports lm.Comol.Modules.EduPath.BusinessLogic.RootObject

Public Class UC_HelpVisibility
    Inherits BaseControlSession


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpList", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub InitDialog()
        With Me.Resource
            .setLabel(Me.LBvisibilityHelp)
            .setLabel(Me.LBswitchOn)
            .setLabel(Me.LBswitchOff)
            IMGswitchOff.ToolTip = Me.Resource.getValue("Blocked.False")
            IMGswitchOff.ImageUrl = ImgBtnBlocked_Off(Me.BaseUrl)
            IMGswitchOn.ToolTip = Me.Resource.getValue("Blocked.True")
            IMGswitchOn.ImageUrl = ImgBtnBlocked_On(Me.BaseUrl)
        End With
    End Sub


End Class