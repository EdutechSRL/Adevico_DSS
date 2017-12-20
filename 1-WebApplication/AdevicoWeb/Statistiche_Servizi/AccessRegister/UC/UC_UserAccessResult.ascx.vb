Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract

Partial Public Class UC_UserAccessResult
    Inherits BaseControlSession
    Implements iViewUserAccessResult

    Public Event ActivateNoPermission()

    Public Property ShowUserNameSearch() As Boolean Implements iViewUserAccessResult.ShowUserNameSearch
        Get
            Return Me.DIVusername.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.DIVusername.Visible = value
            If value Then
                DIVusername.Style.Add("display", "block")
            Else
                DIVusername.Style.Add("display", "none")
            End If
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
        Page.Form.DefaultButton = BTNsearch.UniqueID
        Page.Form.DefaultFocus = Me.TXBsearch.ClientID
    End Sub

    Public Overrides Sub BindDati()
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(Me.LTsearch)
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

    Private Enum GridColumn
        Community = 0
        Owner = 1
        Day = 2
        Hour = 3
        Time = 4
        Details = 5
    End Enum

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As UserAccessResultPresenter
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
    Public ReadOnly Property CurrentPresenter() As UserAccessResultPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserAccessResultPresenter(Me.CurrentContext, Me)
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
    Public ReadOnly Property Ascending() As Boolean Implements iViewUserAccessResult.Ascending
        Get
            If Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements iViewUserAccessResult.Pager
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
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > Me.DDLpage.Items(0).Value)
            Me.DIVpageSize.Style.Add("display", IIf(Me.PGgrid.Visible, "block", "none"))
        End Set
    End Property
    Public Property CurrentView() As viewType Implements iViewUserAccessResult.CurrentView
        Get
            Return Me.ViewState("CurrentView")
        End Get
        Set(ByVal value As viewType)
            Me.ViewState("CurrentView") = value
        End Set
    End Property
    Public ReadOnly Property CurrentOrder() As ResultsOrder Implements iViewUserAccessResult.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Me.Request.QueryString("Order"), ResultsOrder.Hour)
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As Integer Implements iViewUserAccessResult.CurrentPage
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


    Public ReadOnly Property PreLoadedEndDate() As Date? Implements iViewUserAccessResult.PreLoadedEndDate
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
    Public ReadOnly Property PreLoadedStartDate() As Date? Implements iViewUserAccessResult.PreLoadedStartDate
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
    Public ReadOnly Property PreLoadedFromView() As viewType Implements iViewUserAccessResult.PreLoadedFromView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Me.Request.QueryString("FromView"), viewType.None)
        End Get
    End Property


    Public Property ResultsContext() As ResultContextBase Implements iViewUserAccessResult.ResultsContext
        Get
            Dim oContext As New ResultContextBase With {.CommunityID = -1, .UserID = 0}
            If TypeOf Me.ViewState("ResultsContext") Is ResultContextBase Then
                oContext = Me.ViewState("ResultsContext")
            Else
                oContext.FromView = Me.PreLoadedFromView

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
                    oContext.FromView = viewType.None
                Else
                    oContext.FromView = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Me.Request.QueryString("SubView"), viewType.None)
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
        Set(ByVal value As ResultContextBase)
            Me.ViewState("ResultsContext") = value
        End Set
    End Property

    Public Property CurrentPageSize() As Integer Implements iViewUserAccessResult.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Public ReadOnly Property PreLoadedPageSize() As Integer Implements iViewUserAccessResult.PreLoadedPageSize
        Get
            Dim PageSize As Integer = Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return DDLpage.SelectedValue
            Else
                If IsNothing(DDLpage.Items.FindByValue(PageSize)) Then
                    Return DDLpage.SelectedValue
                Else
                    Return PageSize
                End If
            End If
        End Get
    End Property
    Public Property SelectedEndDate() As Date? Implements iViewUserAccessResult.SelectedEndDate
        Get
            Return Me.RDPdataF.SelectedDate
        End Get
        Set(ByVal value As Date?)
            Me.RDPdataF.SelectedDate = value
        End Set
    End Property
    Public Property SelectedStartDate() As Date? Implements iViewUserAccessResult.SelectedStartDate
        Get
            Return Me.RDPdataI.SelectedDate
        End Get
        Set(ByVal value As Date?)
            Me.RDPdataI.SelectedDate = value
        End Set
    End Property
    Public Property AllowPrint() As Boolean Implements iViewUserAccessResult.AllowPrint
        Get
            Return HYPprint.Visible
        End Get
        Set(ByVal value As Boolean)
            HYPprint.Visible = value
        End Set
    End Property
#End Region

#Region "Generic Page Property"
    Protected Friend ReadOnly Property AscendingCss() As String
        Get
            Return "icon orderUp"
        End Get
    End Property
    Protected Friend ReadOnly Property DescendingCss() As String
        Get
            Return "icon orderDown"
        End Get
    End Property

#End Region



    Public Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) Implements iViewUserAccessResult.NoPermissionToAccess
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.NoPermission, CreateObjectList(CommunityID, PersonID, OnPersonID), InteractionType.Generic)
        RaiseEvent ActivateNoPermission()
    End Sub

    Public Sub LoadItems(ByVal TotalTime As TimeSpan, ByVal oList As List(Of lm.Comol.Modules.UsageResults.DomainModel.dtoAccessResult)) Implements iViewUserAccessResult.LoadItems
        Me.SetInternazionalizzazione()
        Me.GDVstatistic.DataSource = oList
        Me.GDVstatistic.DataBind()

        Me.LBtotalTime.Visible = False
        Me.Resource.setLabel(Me.LBtotalTime)
        If Not IsNothing(TotalTime) AndAlso TotalTime.TotalSeconds > 0 Then
            Me.LBtotalTime.Visible = True
            Me.LBtotalTime.Text = String.Format(Me.LBtotalTime.Text, GetTimeTranslatedString(TotalTime))
        End If

    End Sub
    Private Function GetOrderByString(ByVal Order As String) As ResultsOrder
        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Order, ResultsOrder.Hour)
    End Function
    Private Sub GDVstatistic_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oHYPdetails As HyperLink
            Dim oItem As lm.Comol.Modules.UsageResults.DomainModel.dtoAccessResult = TryCast(e.Row.DataItem, lm.Comol.Modules.UsageResults.DomainModel.dtoAccessResult)

            oHYPdetails = e.Row.FindControl("HYPdetails")
            If Not IsNothing(oHYPdetails) Then
                oHYPdetails.NavigateUrl = oItem.NavigateTo
                oHYPdetails.ToolTip = Me.Resource.getValue("GDVstatistic.Details.AccessibleHeaderText")
            End If
        End If
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
                        oHYPasc.CssClass = AscendingCss
                        oHYPasc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)

                        oHYPdesc.CssClass = Me.DescendingCss
                        oHYPdesc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)

                        Dim oQuote As String = """"
                        oLiteral.Text = oLinkbutton.Text & " "
                        oLinkbutton.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText

                        oCell.Controls.Add(New LiteralControl(" "))
                        Dim oAscContext As New ResultContextBase, oDescContext As New ResultContextBase
                        oAscContext = Me.ResultsContext.Clone
                        oAscContext.Order = GetOrderByString(oLinkbutton.CommandArgument)
                        oAscContext.Ascending = True
                        oAscContext.CurrentPage = Me.CurrentPage
                        oDescContext = oAscContext.Clone
                        oDescContext.Ascending = False
                        ' oLinkbutton.CommandArgument = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                oCell.Controls.Add(oLiteral)
                                oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                oCell.Controls.Add(oHYPdesc)
                                '  oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                            Else
                                oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                                '  oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                oCell.Controls.Add(oLiteral)
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else
                            oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                            ' oLinkbutton.PostBackUrl = oHYPasc.NavigateUrl
                            oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
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
    Public Sub NavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType) Implements iViewUserAccessResult.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseUsageResultUrl(oContext, oDestinationView) & "&Page={0}"
    End Sub
    Public Function GetNavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType) As String Implements iViewUserAccessResult.GetNavigationUrl
        Return GetBaseNavigationUrl(oContext, oDestinationView)
    End Function
    Private Function GetBaseNavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = GetBaseUsageResultUrl(oContext, oDestinationView, WithBaseUrl)
        If oContext.CurrentPage = -1 Then
            url &= "&Page={0}"
        ElseIf oContext.CurrentPage >= 0 Then
            url &= "&Page=" & oContext.CurrentPage.ToString
        End If
        Return url
    End Function
    Private Function GetBaseUsageResultUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType, Optional ByVal WithBaseUrl As Boolean = True) As String
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
        If oContext.FromView <> viewType.None Then
            url &= "&FromView=" & oContext.FromView.ToString
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
        'If oContext.FromView <> viewType.None Then
        '    url &= "&FromView=" & oContext.FromView.ToString
        'End If
        If Not oDestinationView = viewType.None Then
            url &= "&View=" & oDestinationView.ToString
        End If
        url &= "&FilterValue=" & oContext.NameSurnameFilter
        url &= "&PageSize=" & Me.CurrentPageSize.ToString
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If


        If oDestinationView <> viewType.None Then
            Select Case oDestinationView
                Case viewType.MyPortalPresence
                    url = "Statistiche_Servizi/AccessRegister/MyPortalAccessRegister.aspx" & url
                Case viewType.MyCommunitiesPresence
                    url = "Statistiche_Servizi/AccessRegister/MyCommunityAccessRegister.aspx" & url
                Case viewType.CurrentCommunityPresence
                    url = "Statistiche_Servizi/AccessRegister/MyCurrentCommunityRegister.aspx" & url
                Case viewType.BetweenDateUsersCommunity
                    url = "Statistiche_Servizi/AccessRegister/FindCommunityUserRegisterByDate.aspx" & url
                Case viewType.BetweenDateUsersPortal
                    url = "Statistiche_Servizi/AccessRegister/FindPortalUserRegisterByDate.aspx" & url
                Case viewType.UsersPortalPresence
                    url = "Statistiche_Servizi/AccessRegister/UsersPortalList.aspx" & url
                Case viewType.UsersCurrentCommunityPresence
                    url = "Statistiche_Servizi/AccessRegister/UsersCurrentCommunityList.aspx" & url
                Case viewType.OtherUserCommunityList
                    url = "Statistiche_Servizi/AccessRegister/OtherCommunityAccessRegister.aspx" & url
                Case viewType.OtherUserPresence
                    url = "Statistiche_Servizi/AccessRegister/OtherUserRegister.aspx" & url
            End Select
        End If
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function


    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String Implements iViewUserAccessResult.GetTimeTranslatedString
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
        Me.SetInternazionalizzazione()
        Me.CurrentPresenter.SearchResults()
    End Sub

    Public Sub ShowSummary(ByVal oSummaryType As iViewUserAccessResult.SummaryType, ByVal UserName As String, ByVal CommunityName As String) Implements iViewUserAccessResult.ShowSummary
        Select Case oSummaryType
            Case iViewUserAccessResult.SummaryType.OwnFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewUserAccessResult.SummaryType.PortalUserFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case iViewUserAccessResult.SummaryType.OwnCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case iViewUserAccessResult.SummaryType.UserCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName, UserName)
            Case iViewUserAccessResult.SummaryType.UserCommunitiesList
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case iViewUserAccessResult.SummaryType.PortalUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewUserAccessResult.SummaryType.CommunityUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case iViewUserAccessResult.SummaryType.PortalBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewUserAccessResult.SummaryType.CommunityBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case Else
        End Select
    End Sub

    Private Sub GDVstatistic_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GDVstatistic.Sorting
        Me.RedirectToUrl(Replace(e.SortExpression, Me.BaseUrl, ""))
        e.Cancel = True
    End Sub

    Public Event ShowBackUrl(url As String, fromView As viewType)
    Public WriteOnly Property SetPreviousURL() As String Implements iViewUserAccessResult.SetPreviousURL
        Set(ByVal value As String)
            RaiseEvent ShowBackUrl(value, Me.CurrentView)
        End Set
    End Property

    Public WriteOnly Property SetPrintUrl() As String Implements iViewUserAccessResult.SetPrintUrl
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

    Public Function GetPrintUrl(ByVal oContext As ResultContextBase, ByVal oView As viewType) As String Implements iViewUserAccessResult.GetPrintUrl
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
        If Not oView = viewType.None Then
            url &= "&View=" & oView.ToString
        End If
        If oContext.FromDate <> Nothing Then
            url &= "&FromDate=" & oContext.FromDate.ToString
        End If
        If oContext.ToDate <> Nothing Then
            url &= "&ToDate=" & oContext.ToDate.ToString
        End If
        If oContext.FromView <> viewType.None Then
            url &= "&FromView=" & oContext.FromView.ToString
        End If
        url &= "&FilterValue=" & oContext.NameSurnameFilter
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        url = Me.BaseUrl & "Statistiche_Servizi/AccessRegister/PrintUserAccessReport.aspx" & url

        Return url
    End Function


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        PageUtility.CurrentModule = PageUtility.GetModule(Services_UserAccessReports.Codex)
    End Sub

    Public Sub AddActionSpecifyFilters(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) Implements iViewUserAccessResult.AddActionSpecifyFilters
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.SpecifyFilters, CreateObjectList(CommunityID, PersonID, OnPersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionViewReport(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) Implements iViewUserAccessResult.AddActionViewReport
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.ViewReport, CreateObjectList(CommunityID, PersonID, OnPersonID), InteractionType.Generic)
    End Sub

    Private Function CreateObjectList(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList = Me.PageUtility.CreateObjectsList(Services_UserAccessReports.ObjectType.Community, CommunityID.ToString)
        If PersonID > 0 Then
            oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, PersonID.ToString))
        End If
        oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, OnPersonID.ToString))
        Return oList
    End Function

    Public ReadOnly Property PreLoadedUserName() As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewUserAccessResult.PreLoadedUserName
        Get
            If IsNothing(Request.QueryString("FilterValue")) Then
                Return ""
            Else
                Return Request.QueryString("FilterValue")
            End If
        End Get
    End Property

    Public Property UserName() As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewUserAccessResult.UserName
        Get
            Dim iResponse As String = Me.TXBsearch.Text
            iResponse = iResponse.Trim
            If iResponse <> "" Then
                iResponse = iResponse.ToLower
            End If
            Return iResponse
        End Get
        Set(ByVal value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property

    Public Sub InitControl(ByVal oContext As ResultContextBase, ByVal CurrentView As viewType) Implements iViewUserAccessResult.InitControl
        Me.SetInternazionalizzazione()
        Me.ViewState("CurrentView") = CurrentView
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub DDLpage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.SetInternazionalizzazione()
        Me.CurrentPresenter.ChangePageSize()
    End Sub

    Public WriteOnly Property ShowUserName() As Boolean Implements lm.Comol.Modules.AccessResults.Presentation.iViewUserAccessResult.ShowUserName
        Set(ByVal value As Boolean)
            Me.GDVstatistic.Columns(Me.GridColumn.Owner).Visible = value
        End Set
    End Property
End Class