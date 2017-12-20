Public Class UC_ModalPlayerHeader
    Inherits FRbaseControl

    Public ReadOnly Property ExitMessage() As String
        Get
            Select Case PageUtility.LinguaCode
                Case "it-IT"
                    Return "Sei sicuro/a di voler chiudere la finestra corrente rischiando di perdere la propria sessione di lavoro ?"
                Case "de-DE"
                    Return "Are you sure to close this window ?"
                Case Else
                    Return "Are you sure to close this window ?"
            End Select
        End Get
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
    
End Class