Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBbaseControl
    Inherits BaseControl

#Region "Implements"

#End Region


#Region "Inherits"
     Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private ReadOnly Property CookieName As String
        Get
            Return "DashboardSettings"
        End Get
    End Property
    Protected Friend Function GetCurrentCookie() As UserCurrentSettings 'Implements IViewBaseDashboardLoader.GetCurrentCookie
        Dim myCookie As HttpCookie = Request.Cookies(CookieName)
        If IsNothing(myCookie) Then
            Return Nothing
        Else
            Dim result As New UserCurrentSettings
            result.IdSelectedTile = CLng(myCookie("IdSelectedTile"))
            result.IdSelectedTag = CLng(myCookie("IdSelectedTag"))
            result.AfterUserLogon = CInt(myCookie("AfterUserLogon"))
            result.GroupBy = CInt(myCookie("GroupBy"))
            result.DefaultNoticeboard = CInt(myCookie("Noticeboard"))
            result.TileNoticeboard = CInt(myCookie("TileNoticeboard"))
            result.CombinedNoticeboard = CInt(myCookie("CombineNoticeboard"))
            result.ListNoticeboard = CInt(myCookie("ListNoticeboard"))
            result.OrderBy = CInt(myCookie("OrderBy"))
            result.View = CInt(myCookie("View"))
            Return result
        End If
    End Function
    Protected Friend Sub SaveCurrentCookie(settings As UserCurrentSettings) 'Implements IViewBaseDashboardLoader.SaveCurrentCookie
        Dim myCookie As HttpCookie = New HttpCookie(CookieName)
        myCookie("IdSelectedTile") = settings.IdSelectedTile
        myCookie("IdSelectedTag") = settings.IdSelectedTag
        myCookie("AfterUserLogon") = CInt(settings.AfterUserLogon)
        myCookie("GroupBy") = CInt(settings.GroupBy)
        myCookie("Noticeboard") = CInt(settings.DefaultNoticeboard)
        myCookie("TileNoticeboard") = CInt(settings.TileNoticeboard)
        myCookie("CombineNoticeboard") = CInt(settings.CombinedNoticeboard)
        myCookie("ListNoticeboard") = CInt(settings.ListNoticeboard)
        myCookie("OrderBy") = CInt(settings.OrderBy)
        myCookie("View") = CInt(settings.View)

        myCookie.Expires = DateTime.Now.AddHours(24)


        If Request.Cookies.AllKeys.Contains(CookieName) Then
            Response.Cookies.Set(myCookie)
        Else
            Response.Cookies.Add(myCookie)
        End If
    End Sub
    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String, Optional removeZero As Boolean = False)
        If datetime.HasValue Then
            Dim time As String = GetTimeToString(datetime, defaultString, removeZero)
            If String.IsNullOrEmpty(time) Then
                Return GetDateToString(datetime, defaultString)
            Else
                Return GetDateToString(datetime, defaultString) & " " & time
            End If
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String, Optional removeZero As Boolean = False)
        If datetime.HasValue Then
            If removeZero AndAlso datetime.Value.Minute = 0 Then
                Return ""
            Else
                Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
            End If
        Else
            Return defaultString
        End If
    End Function
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function


    Protected Friend Function HasGenericConstraints(item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem) As Boolean
        Dim result As Boolean = item.HasConstraints OrElse Not item.Community.AllowUnsubscribe OrElse Not item.Community.AllowSubscription OrElse item.Community.MaxUsersWithDefaultRole > 0 OrElse Not item.Community.IsAvailableForSubscriptionStartOn(DateTime.Now)
        If Not result AndAlso item.Community.IsAvailableForSubscriptionEndOn(DateTime.Now) AndAlso item.Community.SubscriptionEndOn.HasValue AndAlso DateTime.Now().AddMonths(4) > item.Community.SubscriptionEndOn.Value Then
            result = True
        End If
        Return result
    End Function

    Protected Friend Function HasGenericConstraints(item As dtoSubscriptionItem) As Boolean
        Dim result As Boolean = item.HasConstraints OrElse (item.Community.IdType <> StandardCommunityType.Organization AndAlso (Not item.Community.AllowUnsubscribe OrElse Not item.Community.AllowSubscription OrElse item.Community.MaxUsersWithDefaultRole > 0))
        If Not result AndAlso item.Community.IdType <> StandardCommunityType.Organization AndAlso item.Community.IsAvailableForSubscriptionEndOn(DateTime.Now) AndAlso item.Community.SubscriptionEndOn.HasValue AndAlso DateTime.Now().AddMonths(4) > item.Community.SubscriptionEndOn.Value Then
            '           result()
            ' Then
            result = True
        End If
        Return result
    End Function


#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
#End Region

End Class