Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation

Partial Public Class UsageGlobal
	Inherits PageBase
    Implements IviewUsageStatistic


    Private ReadOnly Property DestinationUrl As String
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query)
            Return url
        End Get
    End Property

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As UsageGlobalPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_Statistiche
    Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleStatistics))
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
    Public ReadOnly Property CurrentPresenter() As UsageGlobalPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UsageGlobalPresenter(Me.CurrentContext, Me)
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


    Public ReadOnly Property CurrentPageSize() As Integer Implements IviewUsageStatistic.CurrentPageSize
        Get
            Return 18
        End Get
    End Property
    Public ReadOnly Property Ascending() As Boolean Implements IviewUsageStatistic.Ascending
        Get
            If Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IviewUsageStatistic.Pager
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

    Public Property ViewAvailable() As System.Collections.Generic.IList(Of IviewUsageStatistic.viewType) Implements IviewUsageStatistic.ViewAvailable
        Get
            Dim oList As New List(Of IviewUsageStatistic.viewType)

            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                If oTab.Visible Then
                    oList.Add(oTab.Value)
                End If
            Next
        End Get
        Set(ByVal value As System.Collections.Generic.IList(Of IviewUsageStatistic.viewType))
            Dim ReturnTo As IviewUsageStatistic.viewType = Me.ReturnTo
            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                oTab.Visible = value.Contains(oTab.Value)
                If oTab.Visible Then : oTab.NavigateUrl = Me.CurrentPresenter.GetUrlForTab(oTab.Value, ReturnTo)

                End If
            Next
        End Set
    End Property
    Public Property CurrentView() As IviewUsageStatistic.viewType Implements IviewUsageStatistic.CurrentView
        Get
            If Me.TBSusageTime.SelectedTab Is Nothing Then
                Return IviewUsageStatistic.viewType.None
            Else
                Return Me.TBSusageTime.SelectedTab.Value
            End If
        End Get
        Set(ByVal value As IviewUsageStatistic.viewType)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(value, True)
            If Not IsNothing(oTab) Then
                Me.TBSusageTime.SelectedIndex = oTab.Index
            End If
        End Set
    End Property

    Public ReadOnly Property CurrentOrder() As StatisticOrder Implements IviewUsageStatistic.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.UsageTime)
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As Integer Implements IviewUsageStatistic.CurrentPage
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
    Public ReadOnly Property PreLoadedView() As IviewUsageStatistic.viewType Implements IviewUsageStatistic.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IviewUsageStatistic.viewType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageStatistic.viewType).GetByString(Request.QueryString("View"), IviewUsageStatistic.viewType.Personal)
            End If
        End Get
    End Property
    Public ReadOnly Property ReturnTo() As IviewUsageStatistic.viewType Implements IviewUsageStatistic.ReturnTo
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageStatistic.viewType).GetByString(Request.QueryString("FROM"), IviewUsageStatistic.viewType.None)
        End Get
    End Property
    Public ReadOnly Property StartFrom As IviewUsageStatistic.viewType Implements IviewUsageStatistic.StartFrom
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageStatistic.viewType).GetByString(Request.QueryString("StartFrom"), IviewUsageStatistic.viewType.None)
        End Get
    End Property
    Public Property StatisticContext() As UsageContext Implements IviewUsageStatistic.StatisticContext
        Get
            Dim oContext As New UsageContext With {.CommunityID = -1, .ModuleID = -2, .UserID = 0}
            If TypeOf Me.ViewState("StatisticContext") Is UsageContext Then
                oContext = Me.ViewState("StatisticContext")
            Else
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("CommunityID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("CommunityID").ToList
                        For Each id As String In stringList
                            oContext.CommunityIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("ModuleID")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("ModuleID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("ModuleID").ToList
                        For Each id As String In stringList
                            oContext.ModuleIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try

                End If
                If IsNumeric(Me.Request.QueryString("UserID")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("UserID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("UserID").ToList
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
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc")
                    End If
                End If
                Me.ViewState("StatisticContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As UsageContext)
            Me.ViewState("StatisticContext") = value
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
            Dim oTab As Telerik.Web.UI.RadTab = Nothing
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.CommunityUsers)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.CommunityUsers.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.CommunityUsers.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.GenericCommunity)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.GenericCommunity.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.GenericCommunity.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.GenericSystem)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.GenericSystem.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.GenericSystem.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.GenericUser)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.GenericUser.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.GenericUser.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.Personal)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.Personal.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.Personal.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.PersonalCommunity)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.PersonalCommunity.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.PersonalCommunity.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.SystemUsers)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.SystemUsers.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.SystemUsers.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(IviewUsageStatistic.viewType.UserOnLine)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(IviewUsageStatistic.viewType.UserOnLine.ToString & ".Text")
                oTab.ToolTip = .getValue(IviewUsageStatistic.viewType.UserOnLine.ToString & ".ToolTip")
            End If
            MyBase.Title = .getValue("LBtitolo.text")
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setHyperLink(HYPbackHistory, True, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.AccessNumber, GridColumn.AccessNumber.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Details, GridColumn.Details.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.UsageTime, GridColumn.UsageTime.ToString, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermissionToAccess() Implements IviewUsageStatistic.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub
    Public Sub LoadItems(ByVal oStatistic As dtoStatistic, ByVal oContext As UsageContext, ByVal oType As IviewUsageStatistic.viewType, ByVal oViewDetails As IViewUsageDetails.viewType) Implements IviewUsageStatistic.LoadItems
        Me.GDVstatistic.DataSource = oStatistic.Items
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
                        oHYPasc.ImageUrl = Me.AscendingImage
                        oHYPasc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)

                        oHYPdesc.ImageUrl = Me.DescendingImage
                        oHYPdesc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)

                        'oLinkbutton.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText
                        oCell.Controls.Add(New LiteralControl(" "))
                        Dim oAscContext As New UsageContext, oDescContext As New UsageContext
                        oAscContext = Me.StatisticContext.Clone
                        oAscContext.Order = GetOrderByString(oLinkbutton.CommandArgument)
                        oAscContext.Ascending = True
                        oAscContext.CurrentPage = Me.CurrentPage
                        oDescContext = oAscContext.Clone
                        oDescContext.Ascending = False

                        oLiteral.Text = oLinkbutton.Text & " "
                        oCell.Controls.Add(oLiteral)

                        'oLinkbutton.CommandArgument = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oDescContext, Me.CurrentView, Me.ReturnTo, Me.StartFrom)
                                oCell.Controls.Add(oHYPdesc)
                            Else
                                oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo, Me.StartFrom)
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else

                            oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo, Me.StartFrom)
                            oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oDescContext, Me.CurrentView, Me.ReturnTo, Me.StartFrom)

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
                oHYPdetails.NavigateUrl = oItem.NavigateTo
            End If
        End If
    End Sub

    Public Sub NavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) Implements IviewUsageStatistic.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseStatisticUrl(oDestinationPage, oContext, oDestinationView, oFromView, Me.StartFrom) & "&Page={0}"
    End Sub
    Public Function NavigationUrlToDetails(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oFromView As IviewUsageStatistic.viewType, ByVal oViewDetails As IViewUsageDetails.viewType) As String Implements IviewUsageStatistic.NavigationUrlToDetails
        Dim url As String = Me.GetBaseDetailsUrl(oDestinationPage, oContext, oFromView)
        If oViewDetails <> IViewUsageDetails.viewType.None Then
            url &= "&Show=" & oViewDetails.ToString
        End If
        url = String.Format(url, oContext.CurrentPage)
        Return url

    End Function
    Private Function GetBaseDetailsUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oFromView As IviewUsageStatistic.viewType) As String
        Dim url As String = "?"

        If oFromView <> IviewUsageStatistic.viewType.None Then
            url &= "&From=" & oFromView.ToString
        End If
        'If oContext.Order <> StatisticOrder.None Then
        '	url &= "&Order=" & oContext.Order.ToString
        'End If
        'If oContext.Ascending Then
        '	url &= "&Dir=asc"
        'Else
        '	url &= "&Dir=desc"
        'End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.ModuleID > 0 Then
            url &= "&ModuleID=" & oContext.ModuleID
        End If
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        Select Case oDestinationPage
            Case ViewPage.TimeDetails
                url = Me.BaseUrl & "Statistiche_Servizi/UsageDetails.aspx" & url & "&BackUrl=" & DestinationUrl
            Case ViewPage.Community
                url = Me.BaseUrl & "Statistiche_Servizi/UsageCommunity.aspx" & url
            Case ViewPage.System
                url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
            Case ViewPage.CurrentPage
                url = Me.Request.Url.AbsolutePath & url
        End Select
        Return url
    End Function

    'Private Function BaseUrlOrder(ByVal oOrder As StatisticOrder, ByVal Ascending As Boolean, ByVal oContext As UsageContext) As String
    '	Dim url As String = "?Page=" & Me.Pager.PageIndex

    '	If Me.CurrentView <> IviewUsageStatistic.viewType.None Then
    '		url &= "&View=" & Me.CurrentView.ToString
    '	End If
    '	If oOrder <> StatisticOrder.None Then
    '		url &= "&Order=" & oOrder.ToString
    '	End If
    '	If Ascending Then
    '		url &= "&Dir=asc"
    '	Else
    '		url &= "&Dir=desc"
    '	End If
    '	If oContext.CommunityID > 0 Then
    '		url &= "&CommunityID=" & oContext.CommunityID
    '	End If
    '	If oContext.ModuleID > 0 Then
    '		url &= "&ModuleID=" & oContext.ModuleID
    '	End If
    '	If oContext.UserID > 0 Then
    '		url &= "&UserID=" & oContext.UserID
    '	End If
    '	Return url
    'End Function


    'CORRETTE

    Public Function GetNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType, ByVal startFrom As IviewUsageStatistic.viewType) As String Implements IviewUsageStatistic.GetNavigationUrl

        Return GetBaseNavigationUrl(oDestinationPage, oContext, oDestinationView, oFromView, startFrom)
    End Function
    Private Function GetBaseNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType, ByVal startFrom As IviewUsageStatistic.viewType) As String
        Dim url As String = GetBaseStatisticUrl(oDestinationPage, oContext, oDestinationView, oFromView, startFrom)
        If oContext.CurrentPage = -1 Then
            url &= "&Page={0}"
        ElseIf oContext.CurrentPage >= 0 Then
            url &= "&Page=" & oContext.CurrentPage.ToString
        End If
        Return url
    End Function
    Private Function GetBaseStatisticUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType, ByVal startFrom As IviewUsageStatistic.viewType) As String
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.ModuleID > 0 Then
            url &= "&ModuleID=" & oContext.ModuleID
        End If
        If oContext.Order <> StatisticOrder.None Then
            url &= "&Order=" & oContext.Order.ToString
        End If
        If oContext.Ascending Then
            url &= "&Dir=asc"
        Else
            url &= "&Dir=desc"
        End If
        If oDestinationView <> IviewUsageStatistic.viewType.None Then
            url &= "&View=" & oDestinationView.ToString
        End If
        If oFromView <> IviewUsageStatistic.viewType.None Then
            url &= "&From=" & oFromView.ToString
        End If
        If startFrom <> IviewUsageStatistic.viewType.None Then
            url &= "&StartFrom=" & startFrom.ToString
        Else
            url &= "&StartFrom=" & CurrentView.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        If oDestinationPage <> ViewPage.None Then
            Select Case oDestinationPage
                Case ViewPage.TimeDetails
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageDetails.aspx" & url & "&BackUrl=" & DestinationUrl
                Case ViewPage.Community
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
                Case ViewPage.System
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
                Case ViewPage.CurrentPage
                    url = Me.Request.Url.AbsolutePath & url
                Case ViewPage.OnLineUsers
                    url = Me.BaseUrl & "Statistiche_Servizi/OnLineUsers.aspx" & url
            End Select
        End If

        Return url
    End Function
    Public WriteOnly Property SetPreviousURL(backTo As IviewUsageStatistic.viewType) As String Implements lm.Comol.Modules.UserActions.Presentation.IviewUsageStatistic.SetPreviousURL
        Set(ByVal value As String)
            If value = "" Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = value
                Me.Resource.setHyperLink(HYPbackHistory, backTo.ToString, True, True)
                'Me.HYPbackHistory.Attributes.Add("onclick", "history.go(-1);return false;")
            End If
        End Set
    End Property
    Public Sub LoadSummary(ByVal oSummary As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType) Implements lm.Comol.Modules.UserActions.Presentation.IviewUsageStatistic.LoadSummary
        Dim SummaryString As String = Me.GetSummaryTranslatedString(oSummary, oType)

        Me.LTtotalUsageTime.Text = SummaryString & GetTimeTranslatedString(oSummary.ToTimeSpan)
        Me.HYPdetails.NavigateUrl = oSummary.NavigateTo
    End Sub

    Private Enum GridColumn
        Community = 0
        UsageTime = 1
        AccessNumber = 2
        Owner = 0
        Details = 3
        Guid = 4
        ModuleName = 0
    End Enum

    Public Sub SetFirstColumHeader(ByVal oType As IviewUsageStatistic.viewType) Implements IviewUsageStatistic.SetFirstColumHeader
        Select Case oType
            Case IviewUsageStatistic.viewType.CommunityUsers
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, "Owner", True)
                Me.GDVstatistic.Columns(GridColumn.Owner).SortExpression = StatisticOrder.Owner.ToString
            Case IviewUsageStatistic.viewType.GenericCommunity
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.ModuleName, "ModuleName", True)
                Me.GDVstatistic.Columns(GridColumn.ModuleName).SortExpression = StatisticOrder.ModuleName.ToString
            Case IviewUsageStatistic.viewType.GenericSystem

            Case IviewUsageStatistic.viewType.GenericUser
                If Me.StatisticContext.CommunityID >= 0 Then
                    Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.ModuleName, "ModuleName", True)
                    Me.GDVstatistic.Columns(GridColumn.ModuleName).SortExpression = StatisticOrder.ModuleName.ToString
                Else
                    Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Community, "Community", True)
                    Me.GDVstatistic.Columns(GridColumn.Community).SortExpression = StatisticOrder.Community.ToString
                End If
            Case IviewUsageStatistic.viewType.Personal
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Community, "Community", True)
                Me.GDVstatistic.Columns(GridColumn.Community).SortExpression = StatisticOrder.Community.ToString
            Case IviewUsageStatistic.viewType.PersonalCommunity
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.ModuleName, "ModuleName", True)
                Me.GDVstatistic.Columns(GridColumn.ModuleName).SortExpression = StatisticOrder.ModuleName.ToString
            Case IviewUsageStatistic.viewType.SystemUsers
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, "Owner", True)
                Me.GDVstatistic.Columns(GridColumn.Owner).SortExpression = StatisticOrder.Owner.ToString
            Case IviewUsageStatistic.viewType.UserOnLine
                Me.Resource.setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, "Owner", True)
                Me.GDVstatistic.Columns(GridColumn.Owner).SortExpression = StatisticOrder.Owner.ToString
        End Select
    End Sub

    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String Implements IviewUsageStatistic.GetTimeTranslatedString
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
    Public Function GetSummaryTranslatedString(ByVal oSummary As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType) As String Implements IviewUsageStatistic.GetSummaryTranslatedString
        Dim SummaryString As String = Me.Resource.getValue("summary." & oType.ToString)
        Select Case oType
            Case IviewUsageStatistic.SummaryType.Community
                SummaryString = String.Format(SummaryString, oSummary.CommunityName)
            Case IviewUsageStatistic.SummaryType.CommunityModules
                SummaryString = String.Format(SummaryString, oSummary.CommunityName, oSummary.ModuleName)
            Case IviewUsageStatistic.SummaryType.Modules
                SummaryString = String.Format(SummaryString, oSummary.ModuleName)
            Case IviewUsageStatistic.SummaryType.Personal
                SummaryString = String.Format(SummaryString, oSummary.Owner)
            Case IviewUsageStatistic.SummaryType.PersonalCommunity
                SummaryString = String.Format(SummaryString, oSummary.Owner, oSummary.CommunityName)
            Case IviewUsageStatistic.SummaryType.PersonalCommunityModules
                SummaryString = String.Format(SummaryString, oSummary.Owner, oSummary.CommunityName, oSummary.ModuleName)
            Case IviewUsageStatistic.SummaryType.PersonalModules
                SummaryString = String.Format(SummaryString, oSummary.Owner, oSummary.ModuleName)
            Case IviewUsageStatistic.SummaryType.Portal
        End Select
        Return SummaryString
    End Function

    Private Sub GDVstatistic_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GDVstatistic.Sorting
        'Dim oContex As UsageContext = Me.StatisticContext
        'oContex.Ascending = IIf(e.SortDirection = UI.WebControls.SortDirection.Ascending, True, False)
        'oContex.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(e.SortExpression, StatisticOrder.UsageTime)
        'oContex.CurrentPage = 0
        Me.RedirectToUrl(Replace(e.SortExpression, Me.BaseUrl, ""))
        e.Cancel = True
    End Sub

  
End Class