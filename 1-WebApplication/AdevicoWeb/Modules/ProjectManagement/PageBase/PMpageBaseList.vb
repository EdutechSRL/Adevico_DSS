Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMpageBaseList
    Inherits PMpageBase
    Implements IViewBaseList


#Region "Context"
    Private _Presenter As ProjectsListPresenter
    Protected Friend ReadOnly Property CurrentPresenter() As ProjectsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectsListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadFromCookies As Boolean Implements IViewBaseList.PreloadFromCookies
        Get
            Try
                Return System.Convert.ToBoolean(Me.Request.QueryString("fromCookies"))
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property
    Private ReadOnly Property PreloadGroupBy As ItemsGroupBy Implements IViewBaseList.PreloadGroupBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemsGroupBy).GetByString(Request.QueryString("grp"), ItemsGroupBy.None)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As ProjectFilterBy Implements IViewBaseList.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProjectFilterBy).GetByString(Request.QueryString("fltBy"), ProjectFilterBy.All)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterStatus As ItemListStatus Implements IViewBaseList.PreloadFilterStatus
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemListStatus).GetByString(Request.QueryString("fltSts"), ItemListStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadDisplay As SummaryDisplay Implements IViewBaseList.PreloadDisplay
        Get
            Return SummaryDisplay.All
            'Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryDisplay).GetByString(Request.QueryString("smd"), SummaryDisplay.All)
        End Get
    End Property
    Private ReadOnly Property PreloadTimeLine As SummaryTimeLine Implements IViewBaseList.PreloadTimeLine
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryTimeLine).GetByString(Request.QueryString("tml"), SummaryTimeLine.Week)
        End Get
    End Property

#End Region

#Region "Settings"
    Protected Friend Property CurrentListContext As dtoProjectContext Implements IViewBaseList.CurrentListContext
        Get
            Return ViewStateOrDefault("CurrentListContext", New dtoProjectContext() With {.IdCommunity = PreloadIdCommunity, .isForPortal = PreloadForPortal, .isPersonal = PreloadIsPersonal})
        End Get
        Set(ByVal value As dtoProjectContext)
            Me.ViewState("CurrentListContext") = value
        End Set
    End Property
    Protected Friend Property LastFilterSettings As dtoItemsFilter Implements IViewBaseList.LastFilterSettings
        Get
            Return ViewStateOrDefault("LastFilterSettings", dtoItemsFilter.GenerateForGroup(GetCurrentContainer, ItemsGroupBy.Plain))
        End Get
        Set(ByVal value As dtoItemsFilter)
            Me.ViewState("LastFilterSettings") = value
        End Set
    End Property

#End Region
    Private ReadOnly Property PortalName As String Implements IViewBaseList.PortalName
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
#End Region

#Region "Internal"
   
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
    Private Sub DisplaySessionTimeout() Implements IViewBaseList.DisplaySessionTimeout
        Dim idCommunity As Integer = IIf(PreloadIdContainerCommunity > -1, PreloadIdContainerCommunity, PreloadIdCommunity)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.ProjectListManager(ProjectIdCommunity, forPortal, isPersonal, PreloadFromCookies, 0, PreloadGroupBy, PreloadFilterBy, PreloadFilterStatus, PreloadTimeLine, PreloadDisplay)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBaseList.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

    Public MustOverride Function GetCurrentContainer() As PageContainerType
   
End Class