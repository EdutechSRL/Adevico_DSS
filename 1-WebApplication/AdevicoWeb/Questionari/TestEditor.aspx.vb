Public Class QuestTestEditor
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.MLVeditor.SetActiveView(VIWtest)
        'Me.CTRLeditorQ.InitializeControl()
        'Me.CTRLeditorQ.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
        'Me.CTRLeditorB.InitializeControl()
        'Me.CTRLeditorC.InitializeControl()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVeditor.SetActiveView(VIWempty)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim idType As Integer = PageUtility.CurrentContext.UserContext.UserTypeID

        Return idType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin OrElse idType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator

    End Function

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
End Class