Public Partial Class Redirect
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim oBanner As New COL_Banner

		Try
			Dim IdPersona As Integer
			If Not IsNothing(Session("objPersona")) Then
				IdPersona = Session("objPersona").id

				oBanner.SetUser(IdPersona, 0)
				Response.Redirect("http://www.soc.unitn.it/comol.asp")
			Else
				Exit Sub
			End If
		Catch ex As Exception

        End Try

        'PORCATA IMMONDA MA CHE APPARENTEMENTE FUNZIONA, AL DI LA' DI VISUALIZZAZIONI OSCENE DURANTE IL TRASFERIMENTO:
        'Response.Clear()
        'Response.Write(Me.GetString)
        'Response.End()

        'In pratica restituisco una pagina HTML PURA con un form precompilato
        'ed un javascript che invia il form al caricamento della pagina...
        'PORCATA IMMONDA, ma funziona! (Vedere su Libra)
    End Sub
    Private Function GetString() As String
        Dim str As String
        str = "<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN'>"
        str &= "<html>"
		str &= "<head>"
        str &= "<title>HTMLPage1</title>"
        str &= "</head>"
        str &= "<body MS_POSITIONING='GridLayout' onload='SendForm()'>"
        str &= "<script language='javascript' type='text/javascript'>"
        str &= "function SendForm() {"
        str &= "document.Test.Submit.click()"
        str &= "}"
        str &= "</script>"

        str &= "<form id='Test' name='Test' action='WebForm2.aspx' method='post'>"
        str &= "<input type='hidden' value='10' name='HDNTest' id='HDNTest'>"
        str &= "<input type='submit' id='Submit' name='Submit'>"
        str &= "</form>"

        str &= "</body>"
        str &= "</html>"
        Return str
    End Function
End Class