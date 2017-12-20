Public Class ConvertStats
    Inherits PageBaseEduPath

    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()

        Dim epId As Int64 = 0
        Dim userId As Int32 = 0

        If IsNumeric(Me.TXTpathId.Text) Then
            epId = TXTpathId.Text
        End If

        If IsNumeric(Me.TXTuserId.Text) Then
            userId = TXTuserId.Text
        End If

        'ServiceStat.ConvertStat(Me.CurrentCommunityID, epId, userId, CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)

    End Sub



    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get
            Return lm.Comol.Modules.EduPath.Domain.EPType.None
        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        'CTRLmoduleStatusMessage.Visible = True
        'CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        'MLVcertificationItem.SetActiveView(VIWmessages)
        'CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return False
        End Get
    End Property
End Class