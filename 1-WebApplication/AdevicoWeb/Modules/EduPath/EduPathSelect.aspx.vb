Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Public Class EduPathSelect
    Inherits PageBaseEduPath
    Protected Overrides ReadOnly Property PathType As EPType
        Get
            Return EPType.None
        End Get
    End Property
    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setButtonByValue(Me.BTNerror, "Path", True)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVpathSelect.SetActiveView(VIWerror)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return Permission.Admin
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        MLVpathSelect.SetActiveView(VIWdata)
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpSelect", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPtimeAuto, False, True)
            HYPtimeAuto.NavigateUrl = BaseUrl & RootObject.AddPath(CurrentCommunityID, EPType.TimeAuto, False)
            .setHyperLink(HYPtimeManual, False, True)
            HYPtimeManual.NavigateUrl = BaseUrl & RootObject.AddPath(CurrentCommunityID, EPType.TimeManual, False)
            .setHyperLink(HYPvoteAuto, False, True)
            HYPvoteAuto.NavigateUrl = BaseUrl & RootObject.AddPath(CurrentCommunityID, EPType.MarkAuto, False)
            .setHyperLink(HYPvoteManual, False, True)
            HYPvoteManual.NavigateUrl = BaseUrl & RootObject.AddPath(CurrentCommunityID, EPType.MarkManual, False)
            .setLabel(LBtimeAuto)
            .setLabel(LBtimeManual)
            .setLabel(LBvoteAuto)
            .setLabel(LBvoteManual)
            .setLabel(LBtimeAutoTitle)
            .setLabel(LBtimeManualTitle)
            .setLabel(LBvoteAutoTitle)
            .setLabel(LBvoteManualTitle)
            Me.Master.ServiceTitle = .getValue("Title")
        End With
    End Sub


    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        MLVpathSelect.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class