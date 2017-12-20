Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.ActionDataContract


Partial Public Class PrintUserAccessReport
    Inherits PageBasePopUp
    Implements IviewPrintResults

#Region "Private"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As PrintResultsPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
#End Region

#Region "Public"
    Public ReadOnly Property CurrentPresenter() As PrintResultsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PrintResultsPresenter(Me.CurrentContext, Me)
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
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
   

    Public ReadOnly Property ContextBase() As ResultContextBase Implements IviewPrintResults.ContextBase
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
                oContext.FromView = PreLoadedFromView
                oContext.CurrentPage = 0
                oContext.NameSurnameFilter = PreLoadedUserName
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
    End Property

    Public ReadOnly Property CurrentView() As viewType Implements IviewPrintResults.CurrentView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return viewType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Request.QueryString("View"), viewType.MyPortalPresence)
            End If
        End Get
    End Property
    Private ReadOnly Property PreLoadedFromView() As viewType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of viewType).GetByString(Me.Request.QueryString("FromView"), viewType.None)
        End Get
    End Property
    Private ReadOnly Property PreLoadedUserName() As String
        Get
            If IsNothing(Request.QueryString("FilterValue")) Then
                Return ""
            Else
                Return Request.QueryString("FilterValue")
            End If
        End Get
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Default inherited"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AutoCloseWindow() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageReport", "Statistiche")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(Me.BTNclose, True)
            .setButton(Me.BTNprintItems, True)
            .setLabel(LBuser_t)
            .setLabel(Me.LBdata_t)

            .setHeaderGridView(Me.GDVstatistic, GridColumn.Community, GridColumn.Community.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Day, GridColumn.Day.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Hour, GridColumn.Hour.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Owner, GridColumn.Owner.ToString, True)
            .setHeaderGridView(Me.GDVstatistic, GridColumn.Time, GridColumn.Time.ToString, True)
        End With
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.DIVpermessi.Visible = True
        Me.DIVintestazione.Visible = False
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return Me.CurrentPresenter.HasPermission
    End Function
#End Region

    Public Overrides Sub BindDati()
        Me.DIVintestazione.Visible = True
        Me.DIVpermessi.Visible = False
        Me.CurrentPresenter.LoadItems()
    End Sub

    Public Function GetTimeTranslatedString(ByVal oSpan As System.TimeSpan) As String Implements IviewPrintResults.GetTimeTranslatedString
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

    Public Sub LoadItems(ByVal TotalTime As TimeSpan, ByVal oList As System.Collections.Generic.List(Of lm.Comol.Modules.UsageResults.DomainModel.dtoAccessResult)) Implements IviewPrintResults.LoadItems
        If Me.ContextBase.CommunityID > 0 Then
            Me.GDVstatistic.Columns(GridColumn.Community).Visible = True
        Else
            Me.GDVstatistic.Columns(GridColumn.Community).Visible = False
        End If
        Me.GDVstatistic.DataSource = oList
        Me.GDVstatistic.DataBind()

        Me.LBtotalTime.Visible = False
        Me.Resource.setLabel(Me.LBtotalTime)
        If Not IsNothing(TotalTime) AndAlso TotalTime.TotalSeconds > 0 Then
            Me.LBtotalTime.Visible = True
            Me.LBtotalTime.Text = String.Format(Me.LBtotalTime.Text, GetTimeTranslatedString(TotalTime))
        End If

    End Sub

    Private Enum GridColumn
        Community = 0
        Owner = 1
        Day = 2
        Hour = 3
        Time = 4
    End Enum

    Public WriteOnly Property PrintedBy() As String Implements IviewPrintResults.PrintedBy
        Set(ByVal value As String)
            Me.LBuser.Text = value
        End Set
    End Property
    Public WriteOnly Property PrintedOn() As String Implements IviewPrintResults.PrintedOn
        Set(ByVal value As String)
            Me.LBdata.Text = value
        End Set
    End Property

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_UserAccessReports.Codex)
    End Sub
    Public Sub AddActionPrintReport(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewPrintResults.AddActionPrintReport
        Me.PageUtility.AddAction(Services_UserAccessReports.ActionType.PrintReport, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub
    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IviewPrintResults.AddActionNoPermission
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