Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel

Partial Public Class UC_AccessTab
    Inherits BaseControlWithLoad
    Implements IviewTabAccessResult


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As TabAccessResultPresenter
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
    Public ReadOnly Property CurrentPresenter() As TabAccessResultPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TabAccessResultPresenter(Me.CurrentContext, Me)
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
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Dim oTab As Telerik.Web.UI.RadTab = Nothing
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.MyPortalPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.MyPortalPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.MyPortalPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.CurrentCommunityPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.CurrentCommunityPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.CurrentCommunityPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.MyCommunitiesPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.MyCommunitiesPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.MyCommunitiesPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.UsersCurrentCommunityPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.UsersCurrentCommunityPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.UsersCurrentCommunityPresence.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.UsersPortalPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.UsersPortalPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.UsersPortalPresence.ToString & ".ToolTip")
            End If

            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.BetweenDateUsersPortal)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.BetweenDateUsersPortal.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.BetweenDateUsersPortal.ToString & ".ToolTip")
            End If

            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.BetweenDateUsersCommunity)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.BetweenDateUsersCommunity.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.BetweenDateUsersCommunity.ToString & ".ToolTip")
            End If

            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.OtherUserCommunityList)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.OtherUserCommunityList.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.OtherUserCommunityList.ToString & ".ToolTip")
            End If
            oTab = Me.TBSusageTime.Tabs.FindTabByValue(viewType.OtherUserPresence)
            If Not IsNothing(oTab) Then
                oTab.Text = .getValue(viewType.OtherUserPresence.ToString & ".Text")
                oTab.ToolTip = .getValue(viewType.OtherUserPresence.ToString & ".ToolTip")
            End If
        End With
    End Sub

    Public Property CurrentView() As viewType Implements IviewTabAccessResult.CurrentView
        Get
            If Me.TBSusageTime.SelectedTab Is Nothing Then
                Return viewType.None
            Else
                Return Me.TBSusageTime.SelectedTab.Value
            End If
        End Get
        Set(ByVal value As viewType)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(value, True)
            If Not IsNothing(oTab) Then
                Me.TBSusageTime.SelectedIndex = oTab.Index
            End If
        End Set
    End Property

    Public Sub InitContent(ByVal oContext As ResultContextBase) Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.InitContent
        Me.ResultsContext = oContext
        Me.CurrentPresenter.InitView()
    End Sub

    Public Property ResultsContext() As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.ResultsContext
        Get
            Dim oContext As New ResultContextBase With {.CommunityID = -1, .UserID = 0}
            If TypeOf Me.ViewState("ResultsContext") Is ResultContextBase Then
                oContext = Me.ViewState("ResultsContext")
            End If
            Return oContext
        End Get
        Set(ByVal value As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase)
            Me.ViewState("ResultsContext") = value
        End Set
    End Property

    Public Property ViewAvailable() As System.Collections.Generic.IList(Of lm.Comol.Modules.AccessResults.DomainModel.viewType) Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.ViewAvailable
        Get
            Dim oList As New List(Of viewType)

            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                If oTab.Visible Then
                    oList.Add(oTab.Value)
                End If
            Next
        End Get
        Set(ByVal value As IList(Of viewType))
            For Each oTab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                oTab.Visible = value.Contains(oTab.Value)
                If oTab.Visible Then : oTab.NavigateUrl = Me.CurrentPresenter.GetUrlForTab(oTab.Value)

                End If
            Next
        End Set
    End Property



    Public Function GetNavigationUrl(ByVal oContext As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase, ByVal oDestinationView As lm.Comol.Modules.AccessResults.DomainModel.viewType) As String Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.GetNavigationUrl
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

        If oContext.FromDate <> Nothing Then
            url &= "&FromDate=" & oContext.FromDate.ToString
        End If
        If oContext.ToDate <> Nothing Then
            url &= "&ToDate=" & oContext.ToDate.ToString
        End If

        If oContext.FromView <> viewType.None Then
            url &= "&FromView=" & oContext.FromView.ToString
        End If

        If Not oDestinationView = viewType.None Then
            url &= "&View=" & oDestinationView.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
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
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function
    Public Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer) Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.NoPermissionToAccess

    End Sub

    Public Event ActivateSubView()
    Public Sub LoadSubView() Implements lm.Comol.Modules.AccessResults.Presentation.IviewTabAccessResult.LoadSubView
        RaiseEvent ActivateSubView()
    End Sub
End Class