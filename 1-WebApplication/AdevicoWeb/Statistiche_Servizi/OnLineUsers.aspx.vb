Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract


Partial Public Class OnLineUsers
    Inherits PageBase
    Implements IviewOnLineUsers


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
    Private _Presenter As OnLineUsersPresenter
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
    Public ReadOnly Property CurrentPresenter() As OnLineUsersPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New OnLineUsersPresenter(Me.CurrentContext, Me)
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
    Public Property CurrentPageSize() As Integer Implements IviewOnLineUsers.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Public ReadOnly Property Ascending() As Boolean Implements IviewOnLineUsers.Ascending
        Get
            If Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IviewOnLineUsers.Pager
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

    Public Property ViewAvailable() As System.Collections.Generic.IList(Of IviewUsageStatistic.viewType) Implements IviewOnLineUsers.ViewAvailable
        Get
            Dim oList As New List(Of IviewUsageStatistic.viewType)

            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                If oTab.Visible Then
                    oList.Add(oTab.Value)
                End If
            Next
            Return oList
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
    Public Property CurrentView() As IviewUsageStatistic.viewType Implements IviewOnLineUsers.CurrentView
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

    Public ReadOnly Property CurrentOrder() As StatisticOrder Implements IviewOnLineUsers.CurrentOrder
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.LastAction)
        End Get
    End Property
    Public Property ShowIp() As Boolean Implements IviewOnLineUsers.ShowIp
        Get
            Return ViewStateOrDefault("ShowIp", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowIp") = value
        End Set
    End Property

    Public ReadOnly Property CurrentPage() As Integer Implements IviewOnLineUsers.CurrentPage
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
    Public ReadOnly Property PreLoadedView() As IviewUsageStatistic.viewType Implements IviewOnLineUsers.PreLoadedView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IviewUsageStatistic.viewType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageStatistic.viewType).GetByString(Request.QueryString("View"), IviewUsageStatistic.viewType.Personal)
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedLastUpdate() As DateTime Implements IviewOnLineUsers.PreLoadedLastUpdate
        Get
            If IsNothing(Request.QueryString("LastUpdate")) OrElse IsDate(Request.QueryString("LastUpdate")) = False Then
                Return Now
            Else
                Return CDate(Request.QueryString("LastUpdate"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IviewOnLineUsers.PreLoadedPageSize
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
    Public ReadOnly Property PreLoadedNameSurname() As String Implements IviewOnLineUsers.PreLoadedNameSurname
        Get
            Return Request.QueryString("FindByName")
        End Get
    End Property
    Public Property NameSurnameField() As String Implements IviewOnLineUsers.NameSurnameField
        Get
            If String.IsNullOrEmpty(Me.TXBsearch.Text) Then
                Return ""
            Else
                Return Replace(Trim(Me.TXBsearch.Text), "&", " ")
            End If
        End Get
        Set(ByVal value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property
    Public ReadOnly Property ReturnTo() As IviewUsageStatistic.viewType Implements IviewOnLineUsers.ReturnTo
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewUsageStatistic.viewType).GetByString(Request.QueryString("FROM"), IviewUsageStatistic.viewType.None)
        End Get
    End Property
    Public Property OnlineContext() As OnLineUsersContext Implements IviewOnLineUsers.OnLineContext
        Get
            Dim oContext As New OnLineUsersContext With {.CommunityID = -1, .ModuleID = -2, .UserID = 0}
            If TypeOf Me.ViewState("OnlineContext") Is OnLineUsersContext Then
                oContext = Me.ViewState("OnlineContext")
            Else
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("CommunityID")
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("ModuleID")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("ModuleID")
                    Catch ex As Exception

                    End Try

                End If
                oContext.NameSurnameFilter = PreLoadedNameSurname
                oContext.LastUpdate = PreLoadedLastUpdate
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
                Me.ViewState("OnlineContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As OnLineUsersContext)
            Me.ViewState("OnlineContext") = value
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
        Page.Form.DefaultButton = BTNsearch.UniqueID
        Page.Form.DefaultFocus = Me.TXBsearch.ClientID
        'BTNsearch.
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
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, GridColumn.Owner.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Community, GridColumn.Community.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.ModuleName, GridColumn.ModuleName.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.ActionName, GridColumn.ActionName.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.LastAction, GridColumn.LastAction.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.FirstAction, GridColumn.FirstAction.ToString, True)

            .setLiteral(Me.LTsearch)
            .setButton(Me.BTNsearch, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleOnLineUser.ActionType) Implements IviewOnLineUsers.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleOnLineUser.ObjectType.Community, idCommunity.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region
#Region "Internal"

#End Region
    Public Sub NoPermissionToAccess() Implements IviewOnLineUsers.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub
    Public Sub LoadItems(ByVal oList As List(Of dtoOnLineUser), viewIp As Boolean) Implements IviewOnLineUsers.LoadItems
        If oList.Count = 0 Then
            Me.DDLpage.Visible = False
        Else
            Me.DDLpage.Visible = True
        End If
        Me.GDVstatistic.Visible = True
        Me.PGgrid.Visible = True
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
                        oHYPasc.CssClass = AscendingCss
                        oHYPasc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Ascending)

                        oHYPdesc.CssClass = Me.DescendingCss
                        oHYPdesc.ToolTip = Me.Resource.getValue("Direction." & Comol.Entity.sortDirection.Descending)

                        oLinkbutton.ToolTip = sender.Columns(CurrenColumn).AccessibleHeaderText
                        oCell.Controls.Add(New LiteralControl(" "))
                        Dim oAscContext As New OnLineUsersContext, oDescContext As New OnLineUsersContext
                        oAscContext = Me.OnlineContext.Clone
                        oAscContext.Order = GetOrderByString(oLinkbutton.CommandArgument)
                        oAscContext.Ascending = True
                        oAscContext.CurrentPage = Me.CurrentPage
                        oDescContext = oAscContext.Clone
                        oDescContext.Ascending = False

                        oLiteral.Text = oLinkbutton.Text & " "
                        oCell.Controls.Add(oLiteral)
                        If GetOrderByString(oLinkbutton.CommandArgument) = Me.CurrentOrder Then
                            If Me.Ascending Then
                                oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oDescContext, Me.CurrentView, Me.ReturnTo)
                                oCell.Controls.Add(oHYPdesc)
                            Else
                                oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo)
                                oCell.Controls.Add(oHYPasc)
                            End If
                        Else
                            oHYPasc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oAscContext, Me.CurrentView, Me.ReturnTo)
                            oHYPdesc.NavigateUrl = Me.GetBaseNavigationUrl(ViewPage.CurrentPage, oDescContext, Me.CurrentView, Me.ReturnTo)

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
    'Private Sub GDVstatistic_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVstatistic.RowDataBound
    '	If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
    '		Dim oHYPdetails As HyperLink
    '		'Dim oItem As dtoBaseStatistic = TryCast(e.Row.DataItem, dtoBaseStatistic)

    '		'oHYPdetails = e.Row.FindControl("HYPdetails")
    '		'If Not IsNothing(oHYPdetails) Then
    '		'	oHYPdetails.NavigateUrl = oItem.NavigateTo
    '		'End If
    '	End If
    'End Sub

    Public Sub NavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) Implements IviewOnLineUsers.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseStatisticUrl(oDestinationPage, oContext, oDestinationView, oFromView) & "&Page={0}"
    End Sub
    Public Function GetNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) As String Implements IviewOnLineUsers.GetNavigationUrl

        Return GetBaseNavigationUrl(oDestinationPage, oContext, oDestinationView, oFromView)
    End Function
    Private Function GetBaseNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) As String
        Dim url As String = GetBaseStatisticUrl(oDestinationPage, oContext, oDestinationView, oFromView)
        If oContext.CurrentPage = -1 Then
            url &= "&Page={0}"
        ElseIf oContext.CurrentPage >= 0 Then
            url &= "&Page=" & oContext.CurrentPage.ToString
        End If
        Return url
    End Function
    Private Function GetBaseStatisticUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) As String
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
        url &= "&BackUrl=" & DestinationUrl
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        If oDestinationPage <> ViewPage.None Then
            Select Case oDestinationPage
                Case ViewPage.TimeDetails
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageDetails.aspx" & url
                Case ViewPage.Community
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
                Case ViewPage.System
                    url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
                Case ViewPage.CurrentPage
                    url &= "&FindByName=" & oContext.NameSurnameFilter
                    url &= "&PageSize=" & Me.CurrentPageSize.ToString
                    url &= "&LastUpdate=" & oContext.LastUpdate.ToString
                    url = Me.Request.Url.AbsolutePath & url
                Case ViewPage.OnLineUsers
                    url &= "&FindByName=" & oContext.NameSurnameFilter
                    url &= "&PageSize=" & Me.CurrentPageSize.ToString
                    url &= "&LastUpdate=" & oContext.LastUpdate.ToString
                    url = Me.BaseUrl & "Statistiche_Servizi/OnLineUsers.aspx" & url
            End Select
        End If

        Return url
    End Function
    Public WriteOnly Property SetPreviousURL() As String Implements IviewOnLineUsers.SetPreviousURL
        Set(ByVal value As String)
            If value = "" Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = value
                Me.HYPbackHistory.Attributes.Add("onclick", "history.go(-1);return false;")
            End If
        End Set
    End Property
    Public Sub LoadSummary(ByVal oDto As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType) Implements IviewOnLineUsers.LoadSummary
        Dim SummaryString As String = Me.GetSummaryTranslatedString(oDto, oType)

        Me.LTtotalUsageTime.Text = SummaryString
    End Sub

    Private Enum GridColumn
        Owner = 0
        Community = 1
        FirstAction = 2
        ModuleName = 3
        LastAction = 4
        ActionName = 5
    End Enum

    Public Function GetSummaryTranslatedString(ByVal oDto As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType)
        Dim SummaryString As String = ""
        If oDto.nAccesses = 1 And oDto.UsageTime = 1 Then
            SummaryString = Me.Resource.getValue("summary.1.1." & oType.ToString)
        ElseIf oDto.nAccesses = 1 Then
            SummaryString = Me.Resource.getValue("summary.1.n." & oType.ToString)
        ElseIf oDto.UsageTime = 1 Then
            SummaryString = Me.Resource.getValue("summary.n.1." & oType.ToString)
        End If

        If oType = IviewUsageStatistic.SummaryType.OnLineSystem Then
            SummaryString = String.Format(SummaryString, oDto.nAccesses, oDto.UsageTime)
        Else
            SummaryString = String.Format(SummaryString, oDto.CommunityName, oDto.nAccesses, oDto.UsageTime)
        End If
        Return SummaryString
    End Function

    Public Sub UnLoadItems() Implements lm.Comol.Modules.UserActions.Presentation.IviewOnLineUsers.UnLoadItems
        Me.GDVstatistic.Visible = False
        Me.PGgrid.Visible = False
    End Sub

    Public Sub SendToView(ByVal oView As IviewUsageStatistic.viewType) Implements IviewOnLineUsers.SendToView
        Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(oView)
        If Not IsNothing(oTab) Then
            Response.Redirect(oTab.NavigateUrl, True)
        End If
    End Sub


    Public Function GetLastActionTranslated(ByVal oSpan As TimeSpan) As String
        Dim oDate As DateTime = New DateTime
        Dim UsageString As String = ""
        oDate = oDate.AddSeconds(oSpan.TotalSeconds)
        With oDate
            If .Year > 0 And oSpan.TotalDays >= 365 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Year"), .Year, .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Month > 0 And oSpan.TotalDays >= 30 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Month"), .Month, .Day, .Hour, .Minute, .Second)
            ElseIf .Day > 0 And oSpan.TotalDays >= 1 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Day"), .Day, .Hour, .Minute, .Second)
            ElseIf .Hour > 0 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Hour"), .Hour, .Minute, .Second)
            ElseIf .Minute > 0 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Minute"), .Minute, .Second)
            ElseIf .Second > 0 Then
                UsageString = String.Format(Me.Resource.getValue("LastAction.Second"), .Second)
            Else
                UsageString = " // "
            End If
        End With
        Return UsageString
    End Function

    Private Sub DDLpage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.ChangePageSize()
    End Sub

    Private Sub BTNsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsearch.Click
        Me.CurrentPresenter.FindUsers()
        Me.SetInternazionalizzazione()
    End Sub

    Public ReadOnly Property TranslatedContext() As lm.Comol.Modules.UserActions.DomainModel.dtoTranslatedContext Implements lm.Comol.Modules.UserActions.Presentation.IviewOnLineUsers.TranslatedContext
        Get
            Dim oTranslatedContext As New dtoTranslatedContext
            If Resource Is Nothing Then
                Me.SetCultureSettings()
            End If
            oTranslatedContext.GenericAction = Resource.getValue("Context.GenericAction")
            oTranslatedContext.GenericCommunityName = Resource.getValue("Context.GenericCommunityName")
            oTranslatedContext.GenericModuleName = Resource.getValue("Context.GenericModuleName")
            oTranslatedContext.GenericPerson = Resource.getValue("Context.GenericPerson")
            oTranslatedContext.PortalName = Resource.getValue("Context.PortalName")
            Return oTranslatedContext
        End Get
    End Property


End Class