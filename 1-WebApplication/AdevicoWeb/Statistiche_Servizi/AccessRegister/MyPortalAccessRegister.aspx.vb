Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract

Partial Public Class MyPortalAccessRegister
    Inherits PageBase
    Implements IviewContentPageAccessResults

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As ContentPagePresenter
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
    Public ReadOnly Property CurrentPresenter() As ContentPagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ContentPagePresenter(Me.CurrentContext, Me)
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

#Region "Generic Page Property"
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
            MyBase.Title = Resource.getValue("titolo." & Me.CurrentView.ToString)
            Me.Master.ServiceTitle = Resource.getValue("titolo." & Me.CurrentView.ToString)
            .setHyperLink(HYPbackHistory, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermissionToAccess() Implements IviewContentPageAccessResults.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub

    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements lm.Comol.Modules.AccessResults.Presentation.IviewContentPageAccessResults.AddActionNoPermission

    End Sub

    Public Sub InitContent(ByVal oContex As ResultContextBase) Implements lm.Comol.Modules.AccessResults.Presentation.IviewContentPageAccessResults.InitContent
        Me.CTRLaccessTab.InitContent(oContex)
    End Sub


  
    Public Property ResultsContext() As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase Implements lm.Comol.Modules.AccessResults.Presentation.IviewContentPageAccessResults.ResultsContext
        Get
            Dim oContext As New ResultContextBase With {.CommunityID = -1, .UserID = 0}
            If TypeOf Me.ViewState("ResultsContext") Is ResultContextBase Then
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


                If String.IsNullOrEmpty(Me.Request.QueryString("Order")) Then
                    oContext.Order = ResultsOrder.Hour
                    oContext.Ascending = False
                Else
                    oContext.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ResultsOrder).GetByString(Me.Request.QueryString("Order"), ResultsOrder.Hour)
                    oContext.Ascending = True
                    If String.IsNullOrEmpty(Me.Request.QueryString("Dir")) Then
                    Else
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc")
                    End If
                End If
                Me.ViewState("ResultsContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase)
            Me.ViewState("ResultsContext") = value
        End Set
    End Property

    Public ReadOnly Property CurrentView() As viewType Implements lm.Comol.Modules.AccessResults.Presentation.IviewContentPageAccessResults.CurrentView
        Get
            Return Me.CTRLaccessTab.CurrentView
        End Get
    End Property

    Private Sub CTRLaccessTab_ActivateSubView() Handles CTRLaccessTab.ActivateSubView
        Me.CTRLuserAccessResult.Visible = True
        Me.CTRLuserAccessResult.InitControl(Me.ResultsContext, Me.CurrentView)
    End Sub

    Public Function GetNavigationUrl(ByVal oContext As lm.Comol.Modules.AccessResults.DomainModel.ResultContextBase, ByVal oDestinationView As lm.Comol.Modules.AccessResults.DomainModel.viewType) As String Implements lm.Comol.Modules.AccessResults.Presentation.iViewBaseAccessResult.GetNavigationUrl
        Return ""
    End Function

    Private Sub CTRLuserAccessResult_ActivateNoPermission() Handles CTRLuserAccessResult.ActivateNoPermission
        Me.CTRLuserAccessResult.Visible = False
    End Sub

    
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_UserAccessReports.Codex)
    End Sub

    Private Sub CTRLuserAccessResult_ShowBackUrl(url As String, fromView As viewType) Handles CTRLuserAccessResult.ShowBackUrl
        Me.HYPbackHistory.Visible = Not String.IsNullOrEmpty(url)
        Me.HYPbackHistory.NavigateUrl = url
        Resource.setHyperLink(Me.HYPbackHistory, fromView.ToString, True, True)
    End Sub
    Private Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub
End Class