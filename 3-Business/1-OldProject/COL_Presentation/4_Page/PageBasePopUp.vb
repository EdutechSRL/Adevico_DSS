Public MustInherit Class PageBasePopUp
	Inherits Base
	Implements IviewPopUp

	Public MustOverride ReadOnly Property AutoCloseWindow() As Boolean Implements IviewPopUp.AutoCloseWindow
	Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean Implements IviewPopUp.VerifyAuthentication

	Sub New()
		MyBase.New()
	End Sub

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If VerifyAuthentication Then
			If Not IsSessioneScaduta(True) Then
				If Page.IsPostBack = False Then
					Me.SetInternazionalizzazione()
					If HasPermessi() Then
						Me.BindDati()
					Else
						Me.BindNoPermessi()
					End If
				End If
			Else
				Me.BindNoPermessi()
			End If
		Else
			If Me.Page.IsPostBack = False Then
				Me.SetInternazionalizzazione()
			End If
			Me.BindDati()
		End If
	End Sub

	Public MustOverride Function HasPermessi() As Boolean Implements IviewPopUp.HasPermessi
	Public MustOverride Sub BindNoPermessi() Implements IviewPopUp.BindNoPermessi

	Public Overrides Function IsSessioneScaduta(ByVal RedirectToLogin As Boolean) As Boolean
		Dim isScaduta As Boolean = True

		If Not IsNothing(Me.UtenteCorrente) Then
			If Me.UtenteCorrente.Id > 0 Then
				isScaduta = False
			End If
		End If
		If isScaduta And RedirectToLogin Then
			Dim alertMSG As String
			alertMSG = Me.Resource.getValue("LogoutMessage")
			If alertMSG <> "" Then
				alertMSG = alertMSG.Replace("'", "\'")
			Else
				alertMSG = "Session timeout"
			End If
			If AutoCloseWindow Then
				Response.Write("<script language='javascript'>function AlertLogout(Messaggio){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "this.window.close();" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "');" & "');</script>")
			Else
				Response.Write("<script language='javascript'>function AlertLogout(Messaggio){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "');" & "');</script>")
			End If
			isScaduta = True
		ElseIf Me.isPortalCommunity And Me.ComunitaCorrenteID > 0 Then
			If RedirectToLogin Then
				Me.ExitToLimbo()
			End If
			isScaduta = True
		End If
		Return isScaduta
	End Function

End Class