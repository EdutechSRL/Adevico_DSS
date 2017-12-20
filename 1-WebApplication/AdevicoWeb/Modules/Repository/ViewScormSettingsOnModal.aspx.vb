Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.FileRepository.Domain.ScormSettings
Public Class ScormSettingsOnModal
    Inherits FRscormSettingsPageBase
    Implements IViewScormSettingsView

#Region "Context"
    Private _Presenter As ScormSettingsViewPresenter
    Private ReadOnly Property CurrentPresenter() As ScormSettingsViewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ScormSettingsViewPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadIdSettings As Long Implements IViewScormSettingsView.PreloadIdSettings
        Get
            Return GetLongFromQueryString(QueryKeyNames.idSettings, 0)
        End Get
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CTRLheader.InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "IViewScormSettings")
        CurrentPresenter.InitView(PreloadIdSettings, PreloadIdLink, PreloadIdItem, PreloadIdVersion)
    End Sub

    Public Overrides Sub BindNoPermessi()
        DisplayMessage(Resource.getValue("IViewScormSettingsView.BindNoPermessi"), lm.Comol.Core.DomainModel.Helpers.MessageType.error, True)
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim url As String = DefaultLogoutUrl
        If String.IsNullOrEmpty(url) Then
            url = GetCurrentUrl()
        End If
        RedirectOnSessionTimeOut(url, RepositoryIdCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return Nothing
    End Function

    Protected Friend Overrides Sub DisplayMessage(message As String, mType As lm.Comol.Core.DomainModel.Helpers.MessageType, onEmptyView As Boolean)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(message, mType)
        If onEmptyView Then
            MLVcontent.SetActiveView(VIWempty)
        End If
    End Sub
    Protected Friend Overrides Function GetSettingsControl() As UC_ScormSettings
        Return CTRLsettings
    End Function
#End Region

#Region "Implements"
    Private Sub HideItemsForSessionTimeout() Implements IViewScormSettingsView.HideItemsForSessionTimeout
        DisplayMessage(Resource.getValue("HideItemsForSessionTimeout"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert, True)
    End Sub
#End Region

End Class