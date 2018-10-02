Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class RemotePost
        Private Inputs As System.Collections.Specialized.NameValueCollection = New System.Collections.Specialized.NameValueCollection()
        Public Url As String = ""
        Public Method As String = "post"
        Public FormName As String = "aspnetForm"

        Public Sub Add(ByVal name As String, ByVal value As String)
            Inputs.Add(name, value)
        End Sub

        Public Sub Post()
            System.Web.HttpContext.Current.Response.Clear()
            StandardPost()
        End Sub
        Public Sub Post(Query As String)
            System.Web.HttpContext.Current.Response.Clear()
            If Not String.IsNullOrEmpty(Query) Then
                If (Query.StartsWith("?")) Then
                    Query = Query.Remove(0, 1)
                End If
                Dim cookie = New System.Web.HttpCookie("fileDownload", Query)
                cookie.Expires = Now.AddMinutes(5)
                System.Web.HttpContext.Current.Response.AppendCookie(cookie)
            End If
            StandardPost()
        End Sub

        Private Sub StandardPost()
            Dim quote As String = """"
            System.Web.HttpContext.Current.Response.Write("<html><head>")
            System.Web.HttpContext.Current.Response.Write(String.Format("</head><body onload=" & quote & "document.{0}.submit()" & quote & ">", FormName))
            System.Web.HttpContext.Current.Response.Write(String.Format("<form name=" & quote & "{0}" & quote & " method=" & quote & "{1}" & quote & " action=" & quote & "{2}" & quote & " >", FormName, Method, Url))
            For i As Integer = 0 To Inputs.Keys.Count - 1
                System.Web.HttpContext.Current.Response.Write(String.Format("<input name=" & quote & "{0}" & quote & " type=" & quote & "hidden" & quote & " value=" & quote & "{1}" & quote & ">", Inputs.Keys(i), Inputs(Inputs.Keys(i))))
            Next
            System.Web.HttpContext.Current.Response.Write("</form>")
            System.Web.HttpContext.Current.Response.Write("</body></html>")

            System.Web.HttpContext.Current.Response.End()
        End Sub

       
    End Class
End Namespace