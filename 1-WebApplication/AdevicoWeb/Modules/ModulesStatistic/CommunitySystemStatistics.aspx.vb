﻿
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation

Partial Public Class CommunitySystemStatistics
    Inherits MSpageBase
    Implements IViewCommunitySystemStatistics


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As CommunitySystemStatisticsPresenter
    Private _Servizio As Services_Statistiche
    Private _BaseUrl As String
    Private _PagingUrl As String
#End Region

#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CommunitySystemStatisticsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunitySystemStatisticsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

#Region "View property"
    Private ReadOnly Property CurrentPageSize() As Integer Implements IViewCommunitySystemStatistics.CurrentPageSize
        Get
            Return 25
        End Get
    End Property
    Private Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommunitySystemStatistics.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Private ReadOnly Property CurrentView() As StatisticView Implements IViewCommunitySystemStatistics.CurrentView
        Get
            Return StatisticView.System
        End Get
    End Property

    Private ReadOnly Property CurrentOrder() As StatisticOrder Implements IViewCommunitySystemStatistics.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.UsageTime)
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As Integer Implements IViewCommunitySystemStatistics.CurrentPage
        Get
            If Me.Request.QueryString("Page") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("Page"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedSearchBy() As String Implements IViewCommunitySystemStatistics.PreloadedSearchBy
        Get
            Return Request.QueryString("SearchBy")
        End Get
    End Property
    Private Property StatisticContext() As UsageContext Implements IViewCommunitySystemStatistics.StatisticContext
        Get
            Dim oContext As New UsageContext With {.CommunityID = -1, .ModuleID = -2, .UserID = -1}
            If TypeOf Me.ViewState("StatisticContext") Is UsageContext Then
                oContext = Me.ViewState("StatisticContext")
            Else
                If IsNumeric(Me.Request.QueryString("IdCommunity")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("IdCommunity")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdCommunity").ToList
                        For Each id As String In stringList
                            oContext.CommunityIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("IdModule")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("IdModule")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdModule").ToList
                        For Each id As String In stringList
                            oContext.ModuleIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try

                End If
                If IsNumeric(Me.Request.QueryString("IdUser")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("IdUser")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdUser").ToList
                        For Each id As String In stringList
                            oContext.UserIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("groupBy")) Then
                    Try
                        oContext.GroupBy = Me.Request.QueryString("groupBy")
                    Catch ex As Exception

                    End Try
                End If
                oContext.CurrentPage = 0
                If IsNumeric(Me.Request.QueryString("Page")) Then
                    Try
                        oContext.CurrentPage = Me.Request.QueryString("Page")
                    Catch ex As Exception

                    End Try
                End If

                If String.IsNullOrEmpty(Me.Request.QueryString("Order")) Then
                    oContext.Order = StatisticOrder.UsageTime
                    oContext.Ascending = False
                Else
                    oContext.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.UsageTime)
                    oContext.Ascending = True
                    If String.IsNullOrEmpty(Me.Request.QueryString("Dir")) Then
                        If oContext.Order = StatisticOrder.AccessNumber OrElse oContext.Order = StatisticOrder.UsageTime Then
                            oContext.Ascending = False
                        End If
                    Else
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "true")
                    End If
                End If
                oContext.SearchBy = PreloadedSearchBy
                If Not String.IsNullOrEmpty(oContext.SearchBy) Then
                    CurrentSearchBy = oContext.SearchBy
                End If
                Me.ViewState("StatisticContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As UsageContext)
            Me.ViewState("StatisticContext") = value
        End Set
    End Property
    Private Property CurrentSearchBy As String Implements IViewCommunitySystemStatistics.CurrentSearchBy
        Get
            Return Me.TXBsearch.Text
        End Get
        Set(value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property
#End Region
#Region "Generic Page Property"
    Public ReadOnly Property AscendingImage() As String
        Get
            Return Me.BaseUrl & "images/Grid/Ascending.gif"
        End Get
    End Property
    Public ReadOnly Property DescendingImage() As String
        Get
            Return Me.BaseUrl & "images/Grid/Descending.gif"
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

#Region "Generici pagina"
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageGlobal", "Statistiche")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            MyBase.Title = .getValue("LBtitolo.text")
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            Me.Master.ServiceNopermission = .getValue("LBNopermessi.text")
            '.setHyperLink(HYPbackHistory, True, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Community, "Community", True)
            Me.GDVstatistic.Columns(GridColumn.Community).SortExpression = StatisticOrder.Community.ToString

            .setHeaderGridView(Me.GDVstatistic, GridColumn.AccessNumber, GridColumn.AccessNumber.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Details, GridColumn.Details.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.UsageTime, GridColumn.UsageTime.ToString, True)
            .setLabel(LBsearcyByCommunity_t)
            .setButton(BTNsearchByCommunity)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region
    Public Sub DisplaySessionTimeout() Implements IViewCommunitySystemStatistics.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        If PageUtility.BaseUrl = "/" Then
            dto.DestinationUrl = Request.RawUrl.Replace("//", "/")
        Else
            dto.DestinationUrl = Request.RawUrl.Replace(PageUtility.BaseUrl, "")
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub NoPermissionToAccess() Implements IViewCommunitySystemStatistics.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub
    Private Sub LoadAvailableView(views As List(Of StatisticView)) Implements IViewBaseStatistics.LoadAvailableView
        For Each v As StatisticView In views
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(v.ToString)
            If Not IsNothing(oTab) Then
                oTab.Visible = True
                oTab.Text = Resource.getValue("StatisticView." & v.ToString & ".Text")
                oTab.ToolTip = Resource.getValue("StatisticView." & v.ToString & ".ToolTip")

                Select Case v
                    Case StatisticView.MySystem
                        oTab.NavigateUrl = BaseUrl & RootObject.MySystemStatistics(0, StatisticOrder.UsageTime, False)
                    Case StatisticView.UsersSystem
                        oTab.NavigateUrl = BaseUrl & RootObject.UsersSystemStatistics(0, "", StatisticOrder.UsageTime, False)
                    Case StatisticView.System
                        oTab.NavigateUrl = BaseUrl & RootObject.SystemStatistics(0, "", StatisticOrder.UsageTime, False)
                End Select
            End If
        Next
        Me.TBSusageTime.Visible = (views.Count > 1)
    End Sub
    Private Sub LoadItems(ByVal statistics As dtoStatistic, ByVal ucontext As UsageContext, ByVal vPage As StatisticView, ByVal detailType As DetailViewType) Implements IViewCommunitySystemStatistics.LoadItems
        Me.GDVstatistic.DataSource = statistics.Items
        Me.GDVstatistic.DataBind()
        Me.PGgrid.BaseNavigateUrl = BaseUrl & RootObject.SystemStatistics(GetEncodedText(ucontext.SearchBy), ucontext.Order, ucontext.Ascending)
    End Sub

    Public Sub LoadSummary(ByVal oSummary As dtoSummary, ByVal oType As IViewBaseStatistics.SummaryType) Implements IViewCommunitySystemStatistics.LoadSummary
        Dim SummaryString As String = Me.GetSummaryTranslatedString(oSummary, oType)

        Me.LTtotalUsageTime.Text = SummaryString & GetTimeTranslatedString(oSummary.ToTimeSpan)
        Me.HYPdetails.NavigateUrl = BaseUrl & oSummary.NavigateTo
    End Sub

#Region "Control"
    Private Enum GridColumn
        Community = 0
        UsageTime = 1
        AccessNumber = 2
        Owner = 0
        Details = 3
        Guid = 4
        ModuleName = 0
    End Enum
    Private Sub GDVstatistic_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim CurrenColumn As Integer = 0
            For Each oCell As TableCell In e.Row.Cells
                If oCell.HasControls Then
                    Dim oControl As WebControl = oCell.Controls(0)
                    If TypeOf (oControl) Is System.Web.UI.WebControls.LinkButton Then
                        Dim oLinkbutton As LinkButton = DirectCast(oControl, LinkButton)
                        Dim oLiteral As New Literal

                        Dim oHYPasc As New HyperLink
                        Dim oHYPdesc As New HyperLink
                        oHYPasc.ImageUrl = Me.AscendingImage
                        oHYPasc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)

                        oHYPdesc.ImageUrl = Me.DescendingImage
                        oHYPdesc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)

                        'oLinkbutton.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText
                        oCell.Controls.Add(New LiteralControl(" "))
                        Dim tmpContext As UsageContext = Me.StatisticContext

                        oLiteral.Text = oLinkbutton.Text & " "
                        oCell.Controls.Add(oLiteral)

                        'oLinkbutton.CommandArgument = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                oHYPdesc.NavigateUrl = BaseUrl & RootObject.SystemStatistics(Me.CurrentPage, GetEncodedText(tmpContext.SearchBy), GetOrderByString(oLinkbutton.CommandArgument), False)
                                oCell.Controls.Add(oHYPdesc)
                            Else
                                oHYPasc.NavigateUrl = BaseUrl & RootObject.SystemStatistics(Me.CurrentPage, GetEncodedText(tmpContext.SearchBy), GetOrderByString(oLinkbutton.CommandArgument), True)
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else

                            oHYPasc.NavigateUrl = BaseUrl & RootObject.SystemStatistics(Me.CurrentPage, GetEncodedText(tmpContext.SearchBy), GetOrderByString(oLinkbutton.CommandArgument), True)
                            oHYPdesc.NavigateUrl = BaseUrl & RootObject.SystemStatistics(Me.CurrentPage, GetEncodedText(tmpContext.SearchBy), GetOrderByString(oLinkbutton.CommandArgument), False)

                            oCell.Controls.Add(oHYPasc)
                            oCell.Controls.Add(oHYPdesc)
                        End If
                        oCell.Controls.RemoveAt(0)
                    End If
                End If
                CurrenColumn += 1
            Next
        End If
    End Sub
    Private Function GetOrderByString(ByVal Order As String) As StatisticOrder
        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Order, StatisticOrder.UsageTime)
    End Function
    Private Sub GDVstatistic_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oHYPdetails As HyperLink
            Dim oItem As dtoBaseStatistic = TryCast(e.Row.DataItem, dtoBaseStatistic)

            oHYPdetails = e.Row.FindControl("HYPdetails")
            If Not IsNothing(oHYPdetails) Then
                oHYPdetails.NavigateUrl = IIf(BaseUrl = "" OrElse BaseUrl = "/", ApplicationUrlBase, BaseUrl) & oItem.NavigateTo
                oHYPdetails.Visible = Not String.IsNullOrEmpty(oItem.NavigateTo)
            End If
        End If
    End Sub

    Private Sub GDVstatistic_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GDVstatistic.Sorting
        Me.RedirectToUrl(Replace(e.SortExpression, Me.BaseUrl, ""))
        e.Cancel = True
    End Sub
    Private Sub BTNsearchByCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNsearchByCommunity.Click
        PageUtility.RedirectToUrl(RootObject.SystemStatistics(0, GetEncodedText(CurrentSearchBy), StatisticOrder.UsageTime, False))
    End Sub
#End Region


    Private Function GetEncodedText(ByVal s As String) As String
        If String.IsNullOrEmpty(s) Then
            Return ""
        Else
            Return Server.HtmlEncode(s)
        End If
    End Function
    Private Sub MySystemStatistics_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
End Class