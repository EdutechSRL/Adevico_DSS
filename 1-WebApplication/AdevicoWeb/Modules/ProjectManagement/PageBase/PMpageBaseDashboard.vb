Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMpageBaseDashboard
    Inherits PMpageBase
    Implements IViewBaseDashboard

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadFromCookies As Boolean Implements IViewBaseDashboard.PreloadFromCookies
        Get
            Try
                Return System.Convert.ToBoolean(Me.Request.QueryString("fromCookies"))
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property
    Private ReadOnly Property PreloadTimeLine As SummaryTimeLine Implements IViewBaseDashboard.PreloadTimeLine
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryTimeLine).GetByString(Request.QueryString("tml"), SummaryTimeLine.Week)
        End Get
    End Property
    Private ReadOnly Property PreloadGroupBy As ItemsGroupBy Implements IViewBaseDashboard.PreloadGroupBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemsGroupBy).GetByString(Request.QueryString("grp"), ItemsGroupBy.None)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As ProjectFilterBy Implements IViewBaseDashboard.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProjectFilterBy).GetByString(Request.QueryString("fltBy"), ProjectFilterBy.All)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterStatus As ItemListStatus Implements IViewBaseDashboard.PreloadFilterStatus
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemListStatus).GetByString(Request.QueryString("fltSts"), ItemListStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadDisplay As SummaryDisplay Implements IViewBaseDashboard.PreloadDisplay
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryDisplay).GetByString(Request.QueryString("smd"), SummaryDisplay.All)
        End Get
    End Property
    Protected ReadOnly Property PreloadActivityTimeline As SummaryTimeLine Implements IViewBaseDashboard.PreloadActivityTimeline
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryTimeLine).GetByString(Request.QueryString("aTml"), SummaryTimeLine.Week)
        End Get
    End Property

    Protected ReadOnly Property PreloadUserActivityStatus As UserActivityStatus Implements IViewBaseDashboard.PreloadUserActivityStatus
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of UserActivityStatus).GetByString(Request.QueryString("aSt"), UserActivityStatus.Ignore)
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property DashboardContext As dtoProjectContext Implements IViewBaseDashboard.DashboardContext
        Get
            Return ViewStateOrDefault("DashboardContext", New dtoProjectContext() With {.IdCommunity = PreloadIdCommunity, .isForPortal = PreloadForPortal, .isPersonal = PreloadIsPersonal})
        End Get
        Set(ByVal value As dtoProjectContext)
            Me.ViewState("DashboardContext") = value
        End Set
    End Property
    Protected Friend Property LastFilterSettings As dtoItemsFilter Implements IViewBaseDashboard.LastFilterSettings
        Get
            Return ViewStateOrDefault("LastFilterSettings", dtoItemsFilter.GenerateForGroup(GetCurrentContainer, ItemsGroupBy.Plain))
        End Get
        Set(ByVal value As dtoItemsFilter)
            Me.ViewState("LastFilterSettings") = value
        End Set
    End Property

#End Region
    Private ReadOnly Property PortalName As String Implements IViewBaseDashboard.PortalName
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout(idCommunity As Integer, url As String) Implements IViewBaseDashboard.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Protected Sub RedirectToUrl(url As String) Implements IViewBaseDashboard.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

    Protected Function GetCurrentUrl() As String Implements IViewBaseDashboard.GetCurrentUrl
        Return Request.Url.AbsoluteUri.Replace(PageUtility.ApplicationUrlBase, "")
    End Function
    Public MustOverride Function GetCurrentContainer() As PageContainerType
   
End Class