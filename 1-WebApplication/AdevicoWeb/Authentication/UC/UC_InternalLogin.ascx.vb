Public Class UC_InternalLogin
    Inherits BaseControl

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "Display Ajax"
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterStartupScript(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
   
    Public Sub ReloadCultureSettings()
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
  
End Class