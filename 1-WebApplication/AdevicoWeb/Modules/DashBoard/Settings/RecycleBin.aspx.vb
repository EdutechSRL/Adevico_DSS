Public Class DashboardListRecycleBin
    Inherits DBsettingsPageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadDashboardType, True, PreloadIdCommunity)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.TileList(PreloadDashboardType, True, IdContainerCommunity), IdContainerCommunity)
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.RecycleBin")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.RecycleBin.NoPermission")
            .setHyperLink(HYPgoTo_SettingsListBottom, False, True)
            .setHyperLink(HYPgoTo_SettingsListTop, False, True)
        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem(Optional ByVal top As Boolean = True) As HyperLink
        Return IIf(top, HYPgoTo_SettingsListTop, HYPgoTo_SettingsListBottom)
    End Function
    Protected Friend Overrides Function GetListControl() As UC_SettingsList
        Return CTRLsettings
    End Function
    Protected Friend Overrides Function GetRecycleUrlItem(Optional ByVal top As Boolean = True) As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Function GetAddButton(Optional ByVal top As Boolean = True) As HyperLink
        Return Nothing
    End Function
    Protected Friend Overrides Sub SetTitle(type As lm.Comol.Core.Dashboard.Domain.DashboardType, Optional name As String = "")
        Dim key As String = Resource.getValue("DashboardSettings.ServiceTitle.RecycleBin.DashboardType." & type.ToString)
        Dim nKey As String = Resource.getValue("DashboardSettings.ServiceTitle.RecycleBin.NoPermission.DashboardType." & type.ToString)

        Select Case type
            Case lm.Comol.Core.Dashboard.Domain.DashboardType.Community
                If Not String.IsNullOrEmpty(name) AndAlso key.Contains("{0}") Then
                    Master.ServiceTitle = String.Format(key, name)
                End If
                If Not String.IsNullOrEmpty(name) AndAlso nKey.Contains("{0}") Then
                    Master.ServiceNopermission = String.Format(nKey, name)
                End If
            Case Else
                Master.ServiceTitle = key
                Master.ServiceNopermission = nKey
        End Select
    End Sub
#End Region

#Region "internal"
    Private Sub DashboardListRecycleBin_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Private Sub CTRLsettings_SessionTimeout() Handles CTRLsettings.SessionTimeout
        DisplaySessionTimeout()
    End Sub
    Private Sub CTRLsettings_SettingsLoaded(count As Integer) Handles CTRLsettings.SettingsLoaded
        DVbottomCommands.Visible = (count >= 20)
    End Sub
#End Region

End Class