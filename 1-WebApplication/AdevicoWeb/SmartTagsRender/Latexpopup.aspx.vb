Partial Public Class LatexPopUp
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			TXBlatex.Text = Server.UrlDecode(Request.QueryString.ToString)
			IMGlatex.ImageUrl = "latexImage.aspx?" + Request.QueryString.ToString + "\dpi{300}"
		Catch ex As Exception

		End Try


	End Sub

End Class