﻿Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class CommunityDetails
    Inherits DBpageBase
    Implements IViewCommunityDetailsPage

#Region "Context"
    Private _Presenter As CommunityDetailsPagePresenter
    Private ReadOnly Property CurrentPresenter() As CommunityDetailsPagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityDetailsPagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadFromPage As Boolean Implements IViewCommunityDetailsPage.PreloadFromPage
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("FromPage")) AndAlso Request.QueryString("FromPage").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
    Private ReadOnly Property PreloadFromSession As Boolean Implements IViewCommunityDetailsPage.PreloadFromSession
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("FromSession")) AndAlso Request.QueryString("FromSession").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            url = Request.UrlReferrer.AbsoluteUri
        End If
        CurrentPresenter.InitView(PreloadIdCommunity, PreloadFromPage, url, PreloadFromSession)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        If Not Page.IsPostBack Then
            With Resource
                Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.ViewCommunityDetails")
                Master.ServiceTitleToolTip = .getValue("DashboardSettings.ServiceTitle.ViewCommunityDetails.ToolTip")
                Master.ServiceNopermission = .getValue("DashboardSettings.ServiceTitle.ViewCommunityDetails.NoPermission")
                .setLabel(LBunkownCommunityInfo)
            End With
        End If
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.CommunityDetails(IIf(DashboardIdCommunity > 0, DashboardIdCommunity, PreloadIdCommunity), False), 0, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow)
    End Sub
#End Region

#Region "Implements"
    Private Sub SetTitle(name As String) Implements IViewCommunityDetailsPage.SetTitle
        Master.ServiceTitle = name
    End Sub
    Private Sub InitializeDetails(community As lm.Comol.Core.DomainModel.liteCommunityInfo) Implements IViewCommunityDetailsPage.InitializeDetails
        MLVdetails.SetActiveView(VIWdetails)
        CTRLcommmunityDetails.InitializeControl(community)
    End Sub
    Private Sub DisplayUnknownCommunity() Implements IViewCommunityDetailsPage.DisplayUnknownCommunity
        MLVdetails.SetActiveView(VIWunknown)
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewCommunityDetailsPage.SetBackUrl
        If String.IsNullOrEmpty(url) Then
            HYPgoToPreviousPage.Visible = False
        Else
            HYPgoToPreviousPage.Visible = True
            HYPgoToPreviousPage.NavigateUrl = url
        End If
    End Sub
   
#End Region
#Region "Internal"
    Private Sub ViewCommunityDetails_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        Master.BRheaderActive = False
    End Sub
#End Region
End Class