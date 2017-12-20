Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UsageResults.Presentation
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.UsageResults.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract


Partial Public Class UsageReport
    Inherits PageBase
    Implements IviewUsageResults


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As UsageResultPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
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
    Public ReadOnly Property CurrentPresenter() As UsageResultPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UsageResultPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property


#End Region

#Region "View property"



    Public ReadOnly Property CurrentPageSize() As Integer Implements IviewUsageResults.CurrentPageSize
        Get
            If Me.Request.QueryString("Size") = "" Then
                Return 30
            Else
                Try
                    Return CInt(Me.Request.QueryString("Size"))
                Catch ex As Exception

                End Try
                Return 30
            End If
        End Get
    End Property
    Public ReadOnly Property Ascending() As Boolean Implements IviewUsageResults.Ascending
        Get
            If Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IviewUsageResults.Pager
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
    Public Property ViewAvailable() As System.Collections.Generic.IList(Of IviewUsageResults.viewType) Implements IviewUsageResults.ViewAvailable
        Get
            Dim oList As New List(Of IviewUsageResults.viewType)

            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                If oTab.Visible Then
                    oList.Add(oTab.Value)
                End If
            Next
        End Get
        Set(ByVal value As IList(Of IviewUsageResults.viewType))
            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                oTab.Visible = value.Contains(oTab.Value)
                If oTab.Visible Then : oTab.NavigateUrl = Me.CurrentPresenter.GetUrlForTab(oTab.Value)

                End If
            Next
        End Set
    End Property
    Public Property CurrentView() As IviewUsageResults.viewType Implements IviewUsageResults.CurrentView
        Get
            If Me.TBSusageTime.SelectedTab Is Nothing Then
                Return IviewUsageResults.viewType.None
            Else
                Return Me.TBSusageTime.SelectedTab.Value
            End If
        End Get
        Set(ByVal value As IviewUsageResults.viewType)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(value, True)
            If Not IsNothing(oTab) Then
                Me.TBSusageTime.SelectedIndex = oTab.Index

                MyBase.Title = Resource.getValue("titolo." & value.ToString)
                Me.Master.ServiceTitle = Resource.getValue("titolo." & value.ToString)
            End If
        End Set
    End Property
    Public ReadOnly Property CurrentOrder() As ResultsOrder Implements IviewUsageResults.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Me.Request.QueryString("Order"), ResultsOrder.Hour)
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As Integer Implements IviewUsageResults.CurrentPage
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
    Public ReadOnly Property PreLoadedView() As IviewUsageResults.viewType Implements IviewUsageResults.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IviewUsageResults.viewType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageResults.viewType).GetByString(Request.QueryString("View"), IviewUsageResults.viewType.MyPortalPresence)
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedDetailView() As lm.Comol.Modules.UsageResults.DomainModel.ViewDetailPage Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.PreLoadedDetailView
        Get
            If IsNothing(Request.QueryString("SubView")) Then
                Return ViewDetailPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewDetailPage).GetByString(Request.QueryString("SubView"), ViewDetailPage.None)
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedEndDate() As Date? Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.PreLoadedEndDate
        Get
            Dim EndTate As Date?

            If Not IsNothing(Request.QueryString("ToDate")) Then
                If IsDate(Request.QueryString("ToDate")) Then
                    EndTate = CDate(Request.QueryString("ToDate"))
                End If
            End If
            Return EndTate
        End Get
    End Property

    Public ReadOnly Property PreLoadedStartDate() As Date? Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.PreLoadedStartDate
        Get
            Dim StartDate As Date?

            If Not IsNothing(Request.QueryString("FromDate")) Then
                If IsDate(Request.QueryString("FromDate")) Then
                    StartDate = CDate(Request.QueryString("FromDate"))
                End If
            End If
            Return StartDate
        End Get
    End Property

    Public Property CurrentDetailView() As lm.Comol.Modules.UsageResults.DomainModel.ViewDetailPage Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.CurrentDetailView
        Get
            If TypeOf Me.ViewState("CurrentDetailView") Is ViewDetailPage Then
                Return Me.ViewState("CurrentDetailView")
            Else
                Return ViewDetailPage.None
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.UsageResults.DomainModel.ViewDetailPage)
            Me.ViewState("CurrentDetailView") = value
            Me.ShowDetailView(value)
        End Set
    End Property
    Public Property ResultsContext() As ResultContext Implements IviewUsageResults.ResultsContext
        Get
            Dim oContext As New ResultContext With {.CommunityID = -1, .UserID = 0}
            If TypeOf Me.ViewState("ResultsContext") Is ResultContext Then
                oContext = Me.ViewState("ResultsContext")
            Else
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("CommunityID")
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("UserID")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("UserID")
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

                If Not IsNothing(Request.QueryString("ToDate")) Then
                    If IsDate(Request.QueryString("ToDate")) Then
                        oContext.ToDate = CDate(Request.QueryString("ToDate"))
                    End If
                End If
                If Not IsNothing(Request.QueryString("FromDate")) Then
                    If IsDate(Request.QueryString("FromDate")) Then
                        oContext.FromDate = CDate(Request.QueryString("FromDate"))
                    End If
                End If

                If String.IsNullOrEmpty(Me.Request.QueryString("SubView")) Then
                    oContext.SubView = ViewDetailPage.None
                Else
                    oContext.SubView = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewDetailPage).GetByString(Me.Request.QueryString("SubView"), ViewDetailPage.None)
                End If

                If String.IsNullOrEmpty(Me.Request.QueryString("Order")) Then
                    oContext.Order = ResultsOrder.Hour
                    oContext.Ascending = False
                Else
                    oContext.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Me.Request.QueryString("Order"), ResultsOrder.Hour)
                    oContext.Ascending = True
                    If String.IsNullOrEmpty(Me.Request.QueryString("Dir")) Then
                        'If oContext.Order = StatisticOrder.AccessNumber OrElse oContext.Order = StatisticOrder.UsageTime Then
                        '    oContext.Ascending = False
                        'End If
                    Else
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc")
                    End If
                End If
                Me.ViewState("ResultsContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As ResultContext)
            Me.ViewState("ResultsContext") = value
        End Set
    End Property

    Public Property SelectedEndDate() As Date? Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.SelectedEndDate
        Get
            Return Me.RDPdataF.SelectedDate
        End Get
        Set(ByVal value As Date?)
            Me.RDPdataF.SelectedDate = value
        End Set
    End Property
    Public Property SelectedStartDate() As Date? Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.SelectedStartDate
        Get
            Return Me.RDPdataI.SelectedDate
        End Get
        Set(ByVal value As Date?)
            Me.RDPdataI.SelectedDate = value
        End Set
    End Property
    Public Property AllowPrint() As Boolean Implements IviewUsageResults.AllowPrint
        Get
            Return HYPprint.Visible
        End Get
        Set(ByVal value As Boolean)
            HYPprint.Visible = value
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
            Return True
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
        'Me.RDPdataF.SelectedDate = Nothing
        'Me.RDPdataI.SelectedDate = Nothing
    End Sub
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
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
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Dim oTab As Telerik.Web.UI.RadTab = Nothing
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.MyPortalPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.MyPortalPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.MyPortalPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.CurrentCommunityPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.CurrentCommunityPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.CurrentCommunityPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.MyCommunitiesPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.MyCommunitiesPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.MyCommunitiesPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.UsersCurrentCommunityPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.UsersCurrentCommunityPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.UsersCurrentCommunityPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.UsersPortalPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.UsersPortalPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.UsersPortalPresence.ToString & ".ToolTip")
            End If

            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.BetweenDateUsersPortal)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.BetweenDateUsersPortal.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.BetweenDateUsersPortal.ToString & ".ToolTip")
            End If

            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageResults.viewType.BetweenDateUsersCommunity)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageResults.viewType.BetweenDateUsersCommunity.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageResults.viewType.BetweenDateUsersCommunity.ToString & ".ToolTip")
            End If

            MyBase.Title = .getValue("LBtitolo.text")
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")


            .setButton(Me.BTNsearch)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Community, GridColumn.Community.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Day, GridColumn.Day.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Details, GridColumn.Details.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Hour, GridColumn.Hour.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, GridColumn.Owner.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Time, GridColumn.Time.ToString, True)
            .setLiteral(LTdataI)
            .setLiteral(LTdataF)

            .setRequiredFieldValidator(Me.RFVdadaI, True, False)
            .setRequiredFieldValidator(Me.RFVdadaF, True, False)
            .setCompareValidator(CMVdate)
            .setHyperLink(HYPprint, False, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermissionToAccess() Implements IviewUsageResults.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub

    Public Sub ShowDetailView(ByVal oView As lm.Comol.Modules.UsageResults.DomainModel.ViewDetailPage) Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.ShowDetailView
        Select Case oView
            Case ViewDetailPage.MyCommunityList
                Me.DIVresultFilters.Visible = False
                Me.DIVresultFilters.Style("display") = "none"
            Case ViewDetailPage.UserReport
                Me.DIVresultFilters.Visible = True
                Me.DIVresultFilters.Style("display") = "block"
            Case ViewDetailPage.UsersList
                Me.DIVresultFilters.Visible = False
                Me.DIVresultFilters.Style("display") = "none"
        End Select
    End Sub
    Public Sub LoadItems(ByVal oList As List(Of dtoAccessResult)) Implements IviewUsageResults.LoadItems
        Me.GDVstatistic.DataSource = oList
        Me.GDVstatistic.DataBind()
    End Sub
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
                        '  Dim oHYPlink As New HyperLink
                        oHYPasc.ImageUrl = Me.AscendingImage
                        oHYPasc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)

                        oHYPdesc.ImageUrl = Me.DescendingImage
                        oHYPdesc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)

                        'oHYPlink.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText
                        ' oHYPlink.Text = oLinkbutton.Text
                        Dim oQuote As String = """"


                        'oLiteral.Text = "<a href=" & oQuote & "{0} alt=" & oQuote & sender.Columns(CurrenColumn).AccessibleHeaderText & oQuote & ">" & oLinkbutton.Text & "</a>"
                        oLiteral.Text = oLinkbutton.Text & " "
                        oLinkbutton.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText
                        'oLinkbutton.CausesValidation = False

                        oCell.Controls.Add(New LiteralControl(" "))
                        Dim oAscContext As New ResultContext, oDescContext As New ResultContext
                        oAscContext = Me.ResultsContext.Clone
                        oAscContext.Order = GetOrderByString(oLinkbutton.CommandArgument)
                        oAscContext.Ascending = True
                        oAscContext.CurrentPage = Me.CurrentPage
                        oDescContext = oAscContext.Clone
                        oDescContext.Ascending = False
                        oLinkbutton.CommandArgument = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                ' oLiteral.Text = String.Format(oLiteral.Text, Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView))
                                oCell.Controls.Add(oLiteral)
                                oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                'oHYPlink.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                                'oCell.Controls.Add(oHYPlink)
                                oCell.Controls.Add(oHYPdesc)
                                oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)

                            Else
                                ' oLiteral.Text = String.Format(oLiteral.Text, Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView))

                                oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                                oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                oCell.Controls.Add(oLiteral)
                                'oHYPlink.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                'oCell.Controls.Add(oHYPlink)
                                'oCell.Controls.Add(New LiteralControl(" "))
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else
                            oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                            oLinkbutton.PostBackUrl = oHYPasc.NavigateUrl
                            oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                            ' oLiteral.Text = String.Format(oLiteral.Text, oHYPasc.NavigateUrl)
                            ' oHYPlink.NavigateUrl = oHYPasc.NavigateUrl
                            ' oCell.Controls.Add(oHYPlink)
                            ' oCell.Controls.Add(New LiteralControl(" "))
                            oCell.Controls.Add(oLiteral)
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
    Private Function GetOrderByString(ByVal Order As String) As ResultsOrder
        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Order, ResultsOrder.Hour)
    End Function
    Private Sub GDVstatistic_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oHYPdetails As HyperLink
            Dim oItem As dtoAccessResult = TryCast(e.Row.DataItem, dtoAccessResult)

            oHYPdetails = e.Row.FindControl("HYPdetails")
            If Not IsNothing(oHYPdetails) Then
                oHYPdetails.NavigateUrl = oItem.NavigateTo
                oHYPdetails.ToolTip = Me.Resource.getValue("GDVstatistic.Details.AccessibleHeaderText")
            End If
        End If
    End Sub

    Public Sub NavigationUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType) Implements IviewUsageResults.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseUsageResultUrl(oContext, oDestinationView) & "&Page={0}"
    End Sub
    'Public Function NavigationUrlToDetails(ByVal oContext As ResultContext, ByVal oFromView As IviewUsageResults.viewType, ByVal oViewDetails As ViewDetailPage) As String Implements IviewUsageResults.NavigationUrlToDetails
    '    Dim url As String = Me.GetBaseDetailsUrl(oContext, oFromView, )
    '    If oViewDetails <> ViewDetailPage.None Then
    '        url &= "&Show=" & oViewDetails.ToString
    '    End If
    '    url = String.Format(url, oContext.CurrentPage)
    '    Return url
    'End Function
    'Private Function GetBaseDetailsUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oFromView As IviewUsageStatistic.viewType) As String
    '    Dim url As String = "?"

    '    '    If oFromView <> IviewUsageStatistic.viewType.None Then
    '    '        url &= "&From=" & oFromView.ToString
    '    '    End If
    '    '    If oContext.CommunityID > 0 Then
    '    '        url &= "&CommunityID=" & oContext.CommunityID
    '    '    End If
    '    '    If oContext.ModuleID > 0 Then
    '    '        url &= "&ModuleID=" & oContext.ModuleID
    '    '    End If
    '    '    If oContext.UserID > 0 Then
    '    '        url &= "&UserID=" & oContext.UserID
    '    '    End If
    '    '    If url.StartsWith("?&") Then
    '    '        url = url.Replace("?&", "?")
    '    '    End If
    '    '    Select Case oDestinationPage
    '    '        Case ViewPage.TimeDetails
    '    '            url = Me.BaseUrl & "Statistiche_Servizi/UsageDetails.aspx" & url
    '    '        Case ViewPage.Community
    '    '            url = Me.BaseUrl & "Statistiche_Servizi/UsageCommunity.aspx" & url
    '    '        Case ViewPage.System
    '    '            url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
    '    '        Case ViewPage.CurrentPage
    '    '            url = Me.Request.Url.AbsolutePath & url
    '    '    End Select
    '    If url.StartsWith("?&") Then
    '        url = url.Replace("?&", "?")
    '    End If
    '    url = Me.BaseUrl & "Statistiche_Servizi/UsageReport.aspx" & url

    '    Return url
    'End Function

    Public Function GetNavigationUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType) As String Implements IviewUsageResults.GetNavigationUrl
        Return GetBaseNavigationUrl(oContext, oDestinationView)
    End Function
    Private Function GetBaseNavigationUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = GetBaseUsageResultUrl(oContext, oDestinationView, WithBaseUrl)
        If oContext.CurrentPage = -1 Then
            url &= "&Page={0}"
        ElseIf oContext.CurrentPage >= 0 Then
            url &= "&Page=" & oContext.CurrentPage.ToString
        End If
        Return url
    End Function
    Private Function GetBaseUsageResultUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.Order <> ResultsOrder.None Then
            url &= "&Order=" & oContext.Order.ToString
        End If
        If oContext.SubView <> ViewDetailPage.None Then
            url &= "&SubView=" & oContext.SubView.ToString
        End If
        If oContext.Ascending Then
            url &= "&Dir=asc"
        Else
            url &= "&Dir=desc"
        End If

        If oContext.FromDate <> Nothing Then
            url &= "&FromDate=" & oContext.FromDate.ToString
        End If
        If oContext.ToDate <> Nothing Then
            url &= "&ToDate=" & oContext.ToDate.ToString
        End If

        If Not oDestinationView = IviewUsageResults.viewType.None Then
            url &= "&View=" & oDestinationView.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        If WithBaseUrl Then
            url = Me.BaseUrl & "Statistiche_Servizi/UsageReport.aspx" & url
        Else
            url = "Statistiche_Servizi/UsageReport.aspx" & url
        End If
        Return url
    End Function
    'Public Sub LoadSummary(ByVal oSummary As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType) Implements lm.Comol.Modules.UserActions.Presentation.IviewUsageStatistic.LoadSummary
    '    Dim SummaryString As String = Me.GetSummaryTranslatedString(oSummary, oType)

    '    Me.LTtotalUsageTime.Text = SummaryString & GetTimeTranslatedString(oSummary.ToTimeSpan)
    '    Me.HYPdetails.NavigateUrl = oSummary.NavigateTo
    'End Sub

    Private Enum GridColumn
        Community = 0
        Owner = 1
        Day = 2
        Hour = 3
        Time = 4
        Details = 5
    End Enum

    Public Sub SetFirstColumHeader(ByVal oType As IviewUsageResults.viewType, ByVal oView As ViewDetailPage) Implements IviewUsageResults.SetFirstColumHeader
        For Each o As DataControlField In Me.GDVstatistic.Columns
            o.Visible = False
        Next
        Select Case oView
            Case ViewDetailPage.UserReport
                Me.GDVstatistic.Columns(GridColumn.Day).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Hour).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Time).Visible = True
                'Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, "Owner", True)
            Case ViewDetailPage.UsersList
                Me.GDVstatistic.Columns(GridColumn.Owner).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Details).Visible = True
            Case ViewDetailPage.MyCommunityList
                Me.GDVstatistic.Columns(GridColumn.Community).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Details).Visible = True
            Case ViewDetailPage.BetweenDateReports
                Me.GDVstatistic.Columns(GridColumn.Owner).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Day).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Hour).Visible = True
                Me.GDVstatistic.Columns(GridColumn.Time).Visible = True
        End Select
    End Sub

    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String Implements IviewUsageResults.GetTimeTranslatedString
        Dim oDate As DateTime = New DateTime
        Dim UsageString As String = ""
        oDate = oDate.AddSeconds(oSpan.TotalSeconds)
        With oDate
            If .Year > 0 And oSpan.TotalDays >= 365 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Year"), .Year, .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Month > 0 And oSpan.TotalDays >= 30 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Month"), .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Day > 0 And oSpan.TotalDays >= 1 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Day"), .Day, .Hour, .Minute, .Second)
            ElseIf .Hour > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Hour"), .Hour, .Minute, .Second)
            ElseIf .Minute > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Minute"), .Minute, .Second)
            ElseIf .Second > 0 Then
                UsageString = String.Format(Me.Resource.getValue("UsageTime.Second"), .Second)
            Else
                UsageString = " // "
            End If
        End With
        Return UsageString
    End Function

    Private Sub BTNsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsearch.Click
        Me.ShowDetailView(ViewDetailPage.UserReport)
        Me.CurrentPresenter.SearchResults()
    End Sub

    Public Sub ShowSummary(ByVal oSummaryType As IviewUsageResults.SummaryType, ByVal UserName As String, ByVal CommunityName As String) Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.ShowSummary
        Select Case oSummaryType
            Case IviewUsageResults.SummaryType.OwnFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case IviewUsageResults.SummaryType.PortalUserFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case IviewUsageResults.SummaryType.OwnCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case IviewUsageResults.SummaryType.UserCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName, UserName)
            Case IviewUsageResults.SummaryType.UserCommunitiesList
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case IviewUsageResults.SummaryType.PortalUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case IviewUsageResults.SummaryType.CommunityUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case IviewUsageResults.SummaryType.PortalBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case IviewUsageResults.SummaryType.CommunityBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case Else
        End Select
    End Sub

    Private Sub GDVstatistic_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GDVstatistic.Sorting
        Me.RedirectToUrl(Replace(e.SortExpression, Me.BaseUrl, ""))
        e.Cancel = True
    End Sub

    Public WriteOnly Property SetPreviousURL() As String Implements IviewUsageResults.SetPreviousURL
        Set(ByVal value As String)
            If value = "" Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = value
            End If
        End Set
    End Property

    Public WriteOnly Property SetPrintUrl() As String Implements lm.Comol.Modules.UsageResults.Presentation.IviewUsageResults.SetPrintUrl
        Set(ByVal value As String)
            Me.Resource.setHyperLink(HYPprint, False, True)
            If value = "" Then
                Me.HYPprint.Enabled = False
            Else
                Me.HYPprint.Enabled = True
                Me.HYPprint.NavigateUrl = value
            End If
        End Set
    End Property

    Public Function GetPrintUrl(ByVal oContext As ResultContext, ByVal oView As IviewUsageResults.viewType) As String Implements IviewUsageResults.GetPrintUrl
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.Order <> ResultsOrder.None Then
            url &= "&Order=" & oContext.Order.ToString
        End If
        If oContext.Ascending Then
            url &= "&Dir=asc"
        Else
            url &= "&Dir=desc"
        End If
        url &= "&Page=0"
        If Not oView = IviewUsageResults.viewType.None Then
            url &= "&View=" & oView.ToString
        End If
        If oContext.FromDate <> Nothing Then
            url &= "&FromDate=" & oContext.FromDate.ToString
        End If
        If oContext.ToDate <> Nothing Then
            url &= "&ToDate=" & oContext.ToDate.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        url = Me.BaseUrl & "Statistiche_Servizi/PrintUsageReport.aspx" & url

        Return url
    End Function


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_UserAccessReports.Codex)
    End Sub

    Public Sub AddActionSpecifyFilters(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewUsageResults.AddActionSpecifyFilters
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.SpecifyFilters, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionViewCommunities(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewUsageResults.AddActionViewCommunities
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.ViewCommunities, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionViewReport(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewUsageResults.AddActionViewReport
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.ViewReport, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionViewUsers(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewUsageResults.AddActionViewUsers
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.ViewUsers, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewUsageResults.AddActionNoPermission
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.NoPermission, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub

    Private Function CreateObjectList(ByVal CommunityID As Integer, ByVal PersonID As Integer) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList = Me.PageUtility.CreateObjectsList(Services_UserAccessReports.ObjectType.Community, CommunityID.ToString)
        If PersonID > 0 Then
            oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, PersonID.ToString))
        End If
        Return oList
    End Function
End Class