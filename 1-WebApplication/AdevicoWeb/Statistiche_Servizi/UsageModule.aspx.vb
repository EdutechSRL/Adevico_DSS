Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation

Partial Public Class UsageModule
    Inherits PageBase
    Implements IViewModuleUsage

#Region "View"
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentPager
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
    Public ReadOnly Property Ascending() As Boolean Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.Ascending
        Get

        End Get
    End Property
    Public ReadOnly Property CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CommunityRepositoryPermission
        Get

        End Get
    End Property
    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentCommunityID
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property
    Public Property CurrentEndDate() As WSstatistics.dtoDate Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentEndDate
        Get

        End Get
        Set(ByVal value As WSstatistics.dtoDate)

        End Set
    End Property
    Public Property CurrentModuleID1() As Integer Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentModuleID
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property
    Public ReadOnly Property CurrentOrder() As lm.Comol.Modules.UserActions.DomainModel.StatisticOrder Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentOrder
        Get

        End Get
    End Property
    Public Property CurrentPageSize() As Integer Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentPageSize
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property
    Public Property CurrentStartDate() As WSstatistics.dtoDate Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.CurrentStartDate
        Get

        End Get
        Set(ByVal value As WSstatistics.dtoDate)

        End Set
    End Property
    Public ReadOnly Property PortalName() As String Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.PortalName
        Get

        End Get
    End Property
    Public ReadOnly Property PreloadedCommunityID() As Integer Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.PreloadedCommunityID
        Get

        End Get
    End Property
    Public ReadOnly Property PreloadedModuleID() As Integer Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.PreloadedModuleID
        Get

        End Get
    End Property
    Public Property PreservedDownloadUrl() As String Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.PreservedDownloadUrl
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public ReadOnly Property PreserveDownloadUrl() As Boolean Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.PreserveDownloadUrl
        Get

        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

#Region "Inherited"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        '  Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub


#End Region

    Public Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String
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


    Public Sub LoadItems(ByVal oStatistic As lm.Comol.Modules.UserActions.DomainModel.dtoStatistic) Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.LoadItems

    End Sub
    Public Sub LoadSummary(ByVal CommunityName As String, ByVal ModuleName As String, ByVal TotalTime As System.TimeSpan) Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.LoadSummary

    End Sub
    Public Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.NoPermission

    End Sub



    Public Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.SendActionInit

    End Sub

    Public Sub SendActionLoadItems(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements lm.Comol.Modules.UserActions.Presentation.IViewModuleUsage.SendActionLoadItems

    End Sub
End Class