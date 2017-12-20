Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBpageBaseDashboardLoader
    Inherits DBpageBase
    Implements IViewBaseDashboardLoader


#Region "Implements"

#Region "Preload"
    Protected ReadOnly Property PreloadSettingsBase As ISettingsBase Implements IViewBaseDashboardLoader.PreloadSettingsBase
        Get
            Dim settings As New UserCurrentSettings
            settings.GroupBy = PreloadGroupBy
            Select Case PreloadView
                Case DashboardViewType.List
                    settings.ListNoticeboard = PreloadNoticeboard
                Case DashboardViewType.Tile
                    settings.TileNoticeboard = PreloadNoticeboard
                Case DashboardViewType.Combined
                    settings.CombinedNoticeboard = PreloadNoticeboard
            End Select

            settings.OrderBy = PreloadOrderBy
            settings.View = GetCurrentView()
            Return settings
        End Get
    End Property
#End Region

#End Region

#Region "Internal"
    Private ReadOnly Property PreloadView As DashboardViewType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardViewType).GetByString(Request.QueryString("v"), DashboardViewType.List)
        End Get
    End Property
    Private ReadOnly Property PreloadGroupBy As GroupItemsBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of GroupItemsBy).GetByString(Request.QueryString("g"), GroupItemsBy.None)
        End Get
    End Property
    Private ReadOnly Property PreloadNoticeboard As DisplayNoticeboard
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplayNoticeboard).GetByString(Request.QueryString("n"), DisplayNoticeboard.OnRight)
        End Get
    End Property

    Private ReadOnly Property PreloadOrderBy As OrderItemsBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderItemsBy).GetByString(Request.QueryString("o"), OrderItemsBy.LastAccess)
        End Get
    End Property
    Private ReadOnly Property CookieName As String
        Get
            Return "DashboardSettings"
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Friend Function GetCurrentCookie() As UserCurrentSettings Implements IViewBaseDashboardLoader.GetCurrentCookie
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
    Protected Friend Sub SaveCurrentCookie(settings As ISettingsBase) Implements IViewBaseDashboardLoader.SaveCurrentCookie
        Dim cSettings As UserCurrentSettings = GetCurrentCookie()
        If IsNothing(cSettings) Then
            cSettings = New UserCurrentSettings() With {.View = settings.View, .OrderBy = settings.OrderBy, .DefaultNoticeboard = settings.DefaultNoticeboard, .CombinedNoticeboard = settings.CombinedNoticeboard, .ListNoticeboard = settings.ListNoticeboard, .TileNoticeboard = settings.TileNoticeboard, .GroupBy = settings.GroupBy, .AfterUserLogon = settings.AfterUserLogon}
            SaveCurrentCookie(cSettings)
        Else
            Dim myCookie As HttpCookie = Request.Cookies(CookieName)
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
        End If
    End Sub
    Protected Friend Sub SaveCurrentCookie(settings As UserCurrentSettings) Implements IViewBaseDashboardLoader.SaveCurrentCookie
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
    Protected Friend Function GetAutoLogonCookie() As Helpers.dtoExpiredAccessUrl Implements IViewBaseDashboardLoader.GetAutoLogonCookie
        Return ReadLogoutAccessCookie()
    End Function
    Private Function GetAutoRedirectMode() As Helpers.dtoExpiredAccessUrl.DisplayMode Implements IViewBaseDashboardLoader.GetAutoRedirectMode
        Dim mode As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = Helpers.dtoExpiredAccessUrl.DisplayMode.None
        Dim person As COL_Persona = Me.UtenteCorrente

        Dim dto As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = ReadLogoutAccessCookie()
        If IsNothing(dto) Then
            'Dim oResult As dtoLogoutAccess = Me.ReadLogoutAccessCookie(person.ID, person.Login)
            'If Not IsNothing(oResult) AndAlso oResult.PageUrl <> "" AndAlso oResult.isDownloading Then
            '    mode = dto.Display
            'End If

        ElseIf Not String.IsNullOrEmpty(dto.DestinationUrl) Then
            mode = dto.Display
        End If
        Return mode
    End Function

    Private Sub RedirectToAutoLogonPage(cookie As Helpers.dtoExpiredAccessUrl, redirect As Boolean) Implements IViewBaseDashboardLoader.RedirectToAutoLogonPage
        Dim person As COL_Persona = Me.UtenteCorrente
        If Not IsNothing(person) Then
            Dim display As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
            Dim idCommunity As Integer = 0
            Dim url As String = ""
            If IsNothing(cookie) Then
                Dim oResult As dtoLogoutAccess = Me.ReadLogoutAccessCookie(person.ID, person.Login)
                If Not IsNothing(oResult) AndAlso oResult.PageUrl <> "" AndAlso oResult.isDownloading Then
                    url = oResult.PageUrl
                End If

            ElseIf Not String.IsNullOrEmpty(cookie.DestinationUrl) Then
                If cookie.IdPerson = 0 OrElse cookie.IdPerson = person.ID Then : url = cookie.DestinationUrl

                End If
                idCommunity = cookie.IdCommunity
                display = cookie.Display
            End If
            If Not String.IsNullOrEmpty(url) Then
                Select Case display
                    Case Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
                        'If cookie.IsForDownload Then
                        '    PageUtility.RedirectToUrl(url)
                        'Else
                        PageUtility.WriteAutoOpenWindowCookie(True)
                        If redirect Then
                            If idCommunity > 0 Then
                                Dim oResourceConfig As New ResourceManager
                                oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                                PageUtility.AccessToCommunity(person.ID, idCommunity, oResourceConfig, cookie.PreviousUrl, True)
                            Else
                                PageUtility.RedirectToUrl(cookie.PreviousUrl)
                            End If
                        End If
                        'End If
                    Case Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
                        ClearLogoutAccessCookie()
                        If idCommunity = 0 Then
                            PageUtility.RedirectToUrl(url)
                        ElseIf idCommunity > 0 Then
                            Dim oResourceConfig As New ResourceManager
                            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                            PageUtility.AccessToCommunity(person.ID, idCommunity, oResourceConfig, url, True)
                        End If
                End Select
            End If
        End If
    End Sub

    Private Sub RedirectToAutoLogonPage() Implements IViewBaseDashboardLoader.RedirectToAutoLogonPage
        RedirectToAutoLogonPage(GetAutoLogonCookie, True)
    End Sub
    Private Sub GeneratePortalWebContext(idDefaultOrganization As Integer) Implements IViewBaseDashboardLoader.GeneratePortalWebContext
        Session("AdminForChange") = False
        Session("idComunita_forAdmin") = ""
        Session("CMNT_path_forAdmin") = ""
        Session("CMNT_path_forNews") = ""
        Session("CMNT_ID_forNews") = ""

        Session("IdCmntPadre") = 0
        Session("limbo") = True
        Session("ArrComunita") = Nothing
        Session("IdComunita") = 0
        Session("IdRuolo") = Nothing
        PageUtility.UserDefaultIdOrganization = idDefaultOrganization
        PageUtility.UserCurrentIdOrganization = idDefaultOrganization
        Session("RLPC_id") = Nothing
        Session("TPCM_ID") = -1
    End Sub
#End Region

#Region "MustOverride"
    Protected Friend MustOverride Function GetCurrentView() As DashboardViewType
#End Region


   
End Class