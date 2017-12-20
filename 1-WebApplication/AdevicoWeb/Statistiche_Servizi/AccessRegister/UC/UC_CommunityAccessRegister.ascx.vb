Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract

Partial Public Class UC_CommunityAccessRegister
    Inherits BaseControlSession
    Implements iViewCommunityList

    Public Event ActivateNoPermission()


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As UserAccessCommunityListPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _PagingUrl As String
#End Region

#Region "Base Context"
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
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As UserAccessCommunityListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserAccessCommunityListPresenter(Me.CurrentContext, Me)
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


#Region "view Property"
    Public ReadOnly Property Ascending() As Boolean Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.Ascending
        Get
            If Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CurrentOrder() As lm.Comol.Modules.AccessResults.DomainModel.ResultsOrder Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Me.Request.QueryString("Order"), ResultsOrder.Hour)
        End Get
    End Property

    Public ReadOnly Property CurrentPageIndex() As Integer Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.CurrentPageIndex
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

    Public Property CurrentPageSize() As Integer Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public Property isPersonal() As Boolean Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.isPersonal
        Get
            If TypeOf Me.ViewState("isPersonal") Is Boolean Then
                Return CBool(Me.ViewState("isPersonal"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isPersonal") = value
        End Set
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.Pager
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

    Public Property ResultsContext() As ResultContextBase Implements iViewCommunityList.ResultsContext
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
                oContext.FromView = Me.PreLoadedFromView
                oContext.NameSurnameFilter = Me.PreLoadedCommunityName

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
    Public Event ShowBackUrl(url As String, fromView As viewType)
    Public WriteOnly Property SetPreviousURL() As String Implements iViewCommunityList.SetPreviousURL
        Set(ByVal value As String)
            RaiseEvent ShowBackUrl(value, Me.CurrentView)
        End Set
    End Property
    Public ReadOnly Property PreLoadedCommunityName() As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.PreLoadedCommunityName
        Get
            If IsNothing(Request.QueryString("FilterValue")) Then
                Return ""
            Else
                Return Request.QueryString("FilterValue")
            End If
        End Get
    End Property

    Public ReadOnly Property PreLoadedFromView() As lm.Comol.Modules.AccessResults.DomainModel.viewType Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.PreLoadedFromView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Me.Request.QueryString("FromView"), viewType.None)
        End Get
    End Property

    Public ReadOnly Property PreLoadedPageSize() As Integer Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.PreLoadedPageSize
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
    Public Property CommunityName() As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.CommunityName
        Get
            If Me.TXBsearch.Text <> "" Then
                Return Me.TXBsearch.Text.ToLower.Trim
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property

    Public ReadOnly Property CurrentView() As viewType Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.CurrentView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Me.ViewState("CurrentView"), IIf(Me.isPersonal, viewType.MyCommunitiesPresence, viewType.OtherUserCommunityList))
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
        Me.SetInternazionalizzazione()
        Page.Form.DefaultButton = BTNsearch.UniqueID
        Page.Form.DefaultFocus = Me.TXBsearch.ClientID
    End Sub

#Region "Defaults"
    Public Overrides Sub BindDati()
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(Me.LTsearchCommunityName)
            .setButton(Me.BTNsearch)
            .setHeaderGridView(Me.GDVstatistic, 0, "Community", True)
            .setHeaderGridView(Me.GDVstatistic, 1, "Details", True)
        End With
    End Sub
#End Region

#Region "View Mwthods"
    Public Sub InitControl(ByVal oContext As ResultContextBase, ByVal CurrentView As viewType) Implements iViewCommunityList.InitControl
        Me.SetInternazionalizzazione()
        Me.ViewState("CurrentView") = CurrentView.ToString
        Me.CurrentPresenter.InitView()
    End Sub
    Private Sub BTNsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsearch.Click
        Me.CurrentPresenter.SearchResults(0)
    End Sub
    Public Sub LoadCommunity(ByVal oList As System.Collections.Generic.List(Of dtoCommunityResult)) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.LoadCommunity
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
                        oHYPasc.CssClass = Me.AscendingCss
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
                        oAscContext.CurrentPage = Me.CurrentPageIndex
                        oDescContext = oAscContext.Clone
                        oDescContext.Ascending = False
                        '     oLinkbutton.CommandArgument = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                oCell.Controls.Add(oLiteral)
                                oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                oCell.Controls.Add(oHYPdesc)
                                '    oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                            Else
                                oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                                '    oLinkbutton.PostBackUrl = Me.GetBaseNavigationUrl(oDescContext, Me.CurrentView)
                                oCell.Controls.Add(oLiteral)
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else
                            oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(oAscContext, Me.CurrentView)
                            '  oLinkbutton.PostBackUrl = oHYPasc.NavigateUrl
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
    Private Function GetOrderByString(ByVal Order As String) As ResultsOrder
        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Order, ResultsOrder.Hour)
    End Function
    Private Sub GDVstatistic_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oHYPdetails As HyperLink
            Dim oItem As dtoCommunityResult = TryCast(e.Row.DataItem, dtoCommunityResult)

            oHYPdetails = e.Row.FindControl("HYPdetails")
            If Not IsNothing(oHYPdetails) Then
                oHYPdetails.NavigateUrl = oItem.NavigateTo
                oHYPdetails.ToolTip = Me.Resource.getValue("GDVstatistic.Details.AccessibleHeaderText")
            End If
        End If
    End Sub

    Public Sub ShowSummary(ByVal oSummaryType As lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.SummaryType, ByVal UserName As String, ByVal CommunityName As String) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.ShowSummary
        Select Case oSummaryType
            Case iViewCommunityList.SummaryType.OwnFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewCommunityList.SummaryType.PortalUserFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case iViewCommunityList.SummaryType.OwnCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case iViewCommunityList.SummaryType.UserCommunityFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName, UserName)
            Case iViewCommunityList.SummaryType.UserCommunitiesList
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), UserName)
            Case iViewCommunityList.SummaryType.PortalUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewCommunityList.SummaryType.CommunityUsers
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case iViewCommunityList.SummaryType.PortalBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType))
            Case iViewCommunityList.SummaryType.CommunityBetweenDateFilter
                Me.LTfilterInfo.Text = String.Format(Me.Resource.getValue("summary." & oSummaryType), CommunityName)
            Case Else
        End Select
    End Sub

    Public Sub NavigationUrl(ByVal oContext As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase, ByVal oDestinationView As lm.Comol.Modules.AccessResults.DomainModel.viewType) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseUsageResultUrl(oContext, oDestinationView) & "&Page={0}"
    End Sub

    Public Function GetNavigationUrl(ByVal oContext As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase, ByVal oDestinationView As lm.Comol.Modules.AccessResults.DomainModel.viewType) As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.GetNavigationUrl
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
        If oContext.Ascending Then
            url &= "&Dir=asc"
        Else
            url &= "&Dir=desc"
        End If
        If oContext.FromView <> viewType.None Then
            url &= "&FromView=" & oContext.FromView.ToString
        End If
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
#End Region

    Private Sub DDLpage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.ChangePageSize()
    End Sub

    Public Sub AddActionViewCommunities(ByVal CommunityID As Integer, ByVal PersonViewer As Integer, ByVal CommunityOwnerID As Integer) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.AddActionViewCommunities
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.ViewCommunities, CreateObjectList(CommunityID, PersonViewer, CommunityOwnerID), InteractionType.Generic)
    End Sub
    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonViewer As Integer, ByVal CommunityOwnerID As Integer) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.AddActionNoPermission
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.NoPermission, CreateObjectList(CommunityID, PersonViewer, CommunityOwnerID), InteractionType.Generic)
    End Sub

    Private Function CreateObjectList(ByVal CommunityID As Integer, ByVal PersonViewerID As Integer, ByVal CommunityOwnerID As Integer) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList = Me.PageUtility.CreateObjectsList(Services_UserAccessReports.ObjectType.Community, CommunityID.ToString)
        If PersonViewerID > 0 Then
            oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, PersonViewerID.ToString))
        End If
        If CommunityOwnerID > 0 Then
            oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, CommunityOwnerID.ToString))
        End If
        Return oList
    End Function


    Public ReadOnly Property DefaultPageSize() As Integer
        Get
            Return Me.DDLpage.Items(0).Value
        End Get
    End Property

    Public Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) Implements lm.Comol.Modules.AccessResults.Presentation.iViewCommunityList.NoPermissionToAccess
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.NoPermission, CreateObjectList(CommunityID, PersonID, OnPersonID), InteractionType.Generic)
        RaiseEvent ActivateNoPermission()
    End Sub
End Class