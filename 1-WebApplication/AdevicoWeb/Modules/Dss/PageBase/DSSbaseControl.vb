Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class DSSbaseControl
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private ReadOnly Property CookieBaseName As String
        Get
            Return "comol_FileRepository"
        End Get
    End Property
    'Protected Friend Function GetCurrentCookie() As UserCurrentSettings 'Implements IViewBaseDashboardLoader.GetCurrentCookie
    '    Dim myCookie As HttpCookie = Request.Cookies(CookieName)
    '    If IsNothing(myCookie) Then
    '        Return Nothing
    '    Else
    '        Dim result As New UserCurrentSettings
    '        result.IdSelectedTile = CLng(myCookie("IdSelectedTile"))
    '        result.IdSelectedTag = CLng(myCookie("IdSelectedTag"))
    '        result.AfterUserLogon = CInt(myCookie("AfterUserLogon"))
    '        result.GroupBy = CInt(myCookie("GroupBy"))
    '        result.DefaultNoticeboard = CInt(myCookie("Noticeboard"))
    '        result.TileNoticeboard = CInt(myCookie("TileNoticeboard"))
    '        result.CombinedNoticeboard = CInt(myCookie("CombineNoticeboard"))
    '        result.ListNoticeboard = CInt(myCookie("ListNoticeboard"))
    '        result.OrderBy = CInt(myCookie("OrderBy"))
    '        result.View = CInt(myCookie("View"))
    '        Return result
    '    End If
    'End Function
    'Protected Friend Sub SaveCurrentCookie(settings As UserCurrentSettings) 'Implements IViewBaseDashboardLoader.SaveCurrentCookie
    '    Dim myCookie As HttpCookie = New HttpCookie(CookieName)
    '    myCookie("IdSelectedTile") = settings.IdSelectedTile
    '    myCookie("IdSelectedTag") = settings.IdSelectedTag
    '    myCookie("AfterUserLogon") = CInt(settings.AfterUserLogon)
    '    myCookie("GroupBy") = CInt(settings.GroupBy)
    '    myCookie("Noticeboard") = CInt(settings.DefaultNoticeboard)
    '    myCookie("TileNoticeboard") = CInt(settings.TileNoticeboard)
    '    myCookie("CombineNoticeboard") = CInt(settings.CombinedNoticeboard)
    '    myCookie("ListNoticeboard") = CInt(settings.ListNoticeboard)
    '    myCookie("OrderBy") = CInt(settings.OrderBy)
    '    myCookie("View") = CInt(settings.View)

    '    myCookie.Expires = DateTime.Now.AddHours(24)


    '    If Request.Cookies.AllKeys.Contains(CookieName) Then
    '        Response.Cookies.Set(myCookie)
    '    Else
    '        Response.Cookies.Add(myCookie)
    '    End If
    'End Sub

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function

#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AggregationSelector", "Modules", "Dss")
    End Sub
#End Region

End Class