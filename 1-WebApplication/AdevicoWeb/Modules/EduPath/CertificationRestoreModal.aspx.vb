Public Class CertificationRestoreModal
    Inherits EPpageCertificationRestore

#Region "Inherits"
    Protected Friend Overrides ReadOnly Property IsOnModalWindow As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property IsDownloadPage As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        CTRLmessages.InitializeControl(message, type)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Protected Friend Overrides Sub DisplayCloseButton()

    End Sub
#End Region
    Private Sub LoadCertification_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        Master.EnabledFullWidth = False
    End Sub
End Class