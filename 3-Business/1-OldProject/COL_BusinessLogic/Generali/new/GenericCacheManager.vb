Public Class GenericCacheManager
    Public Shared ReadOnly Property Cache() As System.Web.Caching.Cache
        Get
            Return HttpContext.Current.Cache
        End Get
    End Property

    Public Shared Function EncodeText(ByVal content As String) As String
        content = HttpUtility.HtmlEncode(content)
        content = content.Replace("  ", " &nbsp;&nbsp;").Replace("\n", "<br>")
        Return content
    End Function

    Public Shared Function ConvertNullToEmptyString(ByVal input As String) As String
        If input = Nothing Then
            Return ""
        Else
            Return input
        End If
    End Function

    Public Shared Sub PurgeCacheItems(ByVal prefix As String)
        prefix = prefix.ToLower
        Dim itemsToRemove As New List(Of String)
        Dim enumerator As IDictionaryEnumerator = GenericCacheManager.Cache.GetEnumerator()
        While enumerator.MoveNext
            If enumerator.Key.ToString.ToLower.StartsWith(prefix) Then
                itemsToRemove.Add(enumerator.Key.ToString)
            End If
        End While

        For Each itemToRemove As String In itemsToRemove
            GenericCacheManager.Cache.Remove(itemToRemove)
        Next
    End Sub
End Class
