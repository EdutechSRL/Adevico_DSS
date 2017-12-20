Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.FileRepository.Domain.ScormSettings

Public Class ScormSettings
    Inherits FRscormSettingsPageBase
    Implements IViewScormSettings

#Region "Context"
    Private _Presenter As ScormSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As ScormSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ScormSettingsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowSave As Boolean Implements IViewScormSettings.AllowSave
        Set(value As Boolean)
            BTNsaveScormSettings.Visible = value
        End Set
    End Property

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CTRLheader.InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "IViewScormSettings")
        CurrentPresenter.InitView(PreloadIdLink, PreloadIdItem, PreloadIdVersion, PreloadIdFolder, PreloadIdentifierPath, PreloadSetBackUrl, PreloadBackUrl)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
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
            .setHyperLink(HYPbackToPreviousUrl, False, True)
            .setButton(BTNsaveScormSettings, True)
            Master.ServiceTitle = .getValue("ScormSettings.ServiceTitle")
            Master.ServiceTitleToolTip = .getValue("ScormSettings.ServiceTitle")
            Master.ServiceNopermission = .getValue("ScormSettings.ServiceTitle.NoPermission")

        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return HYPbackToPreviousUrl
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

#Region "Internal"
    Private Sub BTNsaveScormSettings_Click(sender As Object, e As EventArgs) Handles BTNsaveScormSettings.Click
        Dim type As EvaluationType = CTRLsettings.GetPackageEvaluationType
        Dim settings As New List(Of dtoScormItemEvaluationSettings)
        If type <> EvaluationType.FromScormEvaluation Then
            settings = CTRLsettings.GetSettings()
        End If
        If isValidOperation() Then
            If settings.Any(Function(s) s.Error <> ScormSettingsError.none) Then
                DisplayInternalMessage(settings.Where(Function(s) s.Error <> ScormSettingsError.none).GroupBy(Function(s) s.Error).ToDictionary(Function(s) s.Key, Function(s) s.Count), Domain.UserMessageType.scormSettingsErrors)
            Else
                CurrentPresenter.Save(IdItem, IdVersion, IdSettings, type, settings)
            End If
        End If
    End Sub
#End Region

    Private Sub RepositoryItemEdit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub

End Class