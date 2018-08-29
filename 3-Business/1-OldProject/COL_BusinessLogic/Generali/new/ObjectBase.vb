<Serializable()>
 Public MustInherit Class ObjectBase
    Inherits ValueObject(Of ObjectBase)


    Protected Shared ReadOnly Property Cache() As System.Web.Caching.Cache
        Get
            Return HttpContext.Current.Cache
        End Get
    End Property

    Protected Shared Function EncodeText(ByVal content As String) As String
        content = HttpUtility.HtmlEncode(content)
        content = content.Replace("  ", " &nbsp;&nbsp;").Replace("\n", "<br>")
        Return content
    End Function

    Protected Shared Function ConvertNullToEmptyString(ByVal input As String) As String
        If input = Nothing Then
            Return ""
        Else
            Return input
        End If
    End Function

    Protected Shared Sub PurgeCacheItems(ByVal prefix As String)
        prefix = prefix.ToLower
        Dim itemsToRemove As New List(Of String)

        Dim enumerator As IDictionaryEnumerator = ObjectBase.Cache.GetEnumerator()
        While enumerator.MoveNext
            If enumerator.Key.ToString.ToLower.StartsWith(prefix) Then
                itemsToRemove.Add(enumerator.Key.ToString)
            End If
        End While

        For Each itemToRemove As String In itemsToRemove
            ObjectBase.Cache.Remove(itemToRemove)
        Next
    End Sub
    Protected Shared Sub PurgeCacheItems(ByVal startPrefix As String, ByVal endPrefix As String)
        startPrefix = startPrefix.ToLower
        endPrefix = endPrefix.ToLower

        Dim itemsToRemove As New List(Of String)
        Dim enumerator As IDictionaryEnumerator = ObjectBase.Cache.GetEnumerator()
        While enumerator.MoveNext
            If enumerator.Key.ToString.ToLower.StartsWith(startPrefix) AndAlso enumerator.Key.ToString.ToLower.EndsWith(endPrefix) Then
                itemsToRemove.Add(enumerator.Key.ToString)
            End If
        End While
        For Each itemToRemove As String In itemsToRemove
            ObjectBase.Cache.Remove(itemToRemove)
        Next
    End Sub
End Class