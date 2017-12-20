Public Class BaseMasterPage
    Inherits System.Web.UI.MasterPage

#Region "Context"

#End Region

#Region "Skin Management"
    Protected Friend ReadOnly Property PreloadedIdLogonSkin As Long
        Get
            If IsNumeric(Me.Request.QueryString("idLogonSkin")) Then
                Return CLng(Me.Request.QueryString("idLogonSkin"))
            Else
                Return -1
            End If
        End Get
    End Property
    Public Function GetIdLogonSkin() As Long
        Dim idSkin As Long = PreloadedIdLogonSkin
        If idSkin < 1 Then
            idSkin = ReadLogonSkinCookie()
        Else
            WriteLogonSkinCookie(idSkin)
        End If
        Return idSkin
    End Function
    Public Sub WriteLogonSkinCookie(ByVal idLogonSkin As Long)
        Dim oHttpCookie As New HttpCookie("LogonSkin")
        oHttpCookie.Expires = Now.AddDays(1)
        oHttpCookie.Values.Add("idLogonSkin", idLogonSkin)
        If (From k In Response.Cookies.AllKeys Where k = "LogonSkin").Any Then
            Response.Cookies.Set(oHttpCookie)
        Else
            Response.Cookies.Add(oHttpCookie)
        End If
    End Sub
    Public Function ReadLogonSkinCookie() As Long
        Dim idLogonSkin As Long = 0
        Dim oValues As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogonSkin").Values.Keys
                oValues(key) = Request.Cookies("LogonSkin").Values(key)
            Next
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            If IsNumeric(oValues.Item("idLogonSkin")) Then
                idLogonSkin = oValues.Item("idLogonSkin")
            End If
        End If
        Return idLogonSkin
    End Function
    Public Sub ClearCookie(name As String)
        Response.Cookies("name").Expires = Now.AddDays(-1)
    End Sub
#End Region


End Class