Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_ProjectDateInfo
    Inherits BaseControl
    Implements IViewProjectDateInfo

#Region "Context"
    Private _Presenter As ProjectDateInfoPresenter
    Private ReadOnly Property CurrentPresenter() As ProjectDateInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectDateInfoPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property PRdeadline As DateTime? Implements IViewProjectDateInfo.PRdeadline
        Get
            Return ViewState("PRdeadline")
        End Get
        Set(value As DateTime?)
            ViewState("PRdeadline") = value
        End Set
    End Property
    Private Property PRendDate As DateTime? Implements IViewProjectDateInfo.PRendDate
        Get
            Return ViewState("PRendDate")
        End Get
        Set(value As DateTime?)
            ViewState("PRdeadline") = value
        End Set
    End Property
    Private Property PRstartDate As DateTime? Implements IViewProjectDateInfo.PRstartDate
        Get
            Return ViewState("PRstartDate")
        End Get
        Set(value As DateTime?)
            ViewState("PRstartDate") = value
        End Set
    End Property
    Public ReadOnly Property InEditStartDate As DateTime? Implements IViewProjectDateInfo.InEditStartDate
        Get
            If CTRLstartDateInput.IsUpdated AndAlso CTRLstartDateInput.IsValid Then
                If Not String.IsNullOrEmpty(CTRLstartDateInput.NewValue) AndAlso CTRLstartDateInput.IsValid Then
                    Dim inEdit As DateTime? = GetDateFromString(CTRLstartDateInput.NewValue, PRstartDate)

                    If inEdit.HasValue AndAlso inEdit.Value.Hour = 0 Then
                        inEdit = inEdit.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
                    End If
                    Return inEdit
                Else
                    Return PRstartDate
                End If
            Else : Return PRstartDate
            End If
        End Get

    End Property
    Public ReadOnly Property InEditDeadline As DateTime? Implements IViewProjectDateInfo.InEditDeadline
        Get
            If CTRLdeadlineInput.IsUpdated AndAlso CTRLdeadlineInput.IsValid Then

                If String.IsNullOrEmpty(CTRLdeadlineInput.NewValue) Then
                    Return Nothing
                ElseIf Not String.IsNullOrEmpty(CTRLdeadlineInput.NewValue) AndAlso CTRLdeadlineInput.IsValid Then

                    Dim inEdit As DateTime? = GetDateFromString(CTRLdeadlineInput.NewValue, PRdeadline)

                    If inEdit.HasValue AndAlso inEdit.Value.Hour = 0 Then
                        inEdit = inEdit.Value.AddHours(DefaultWorkingDay.DefaultStart.FromHour)
                    End If
                    Return inEdit
                Else
                    Return PRdeadline
                End If
            Else : Return PRdeadline
            End If
        End Get
    End Property
    Public Property CurrentShortDatePattern As String Implements IViewProjectDateInfo.CurrentShortDatePattern
        Get
            Return ViewStateOrDefault("CurrentShortDatePattern", LoaderCultureInfo.DateTimeFormat.ShortDatePattern)
        End Get
        Set(value As String)
            ViewState("CurrentShortDatePattern") = value
        End Set
    End Property
    Private Property DefaultWorkingDay As dtoWorkingDay Implements IViewProjectDateInfo.DefaultWorkingDay
        Get
            Return ViewStateOrDefault("DefaultWorkingDay", dtoWorkingDay.GetDefault())
        End Get
        Set(value As dtoWorkingDay)
            ViewState("DefaultWorkingDay") = value
        End Set
    End Property
    Private Property AllowEdit As Boolean Implements IViewProjectDateInfo.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Public Property LoaderCultureInfo As System.Globalization.CultureInfo Implements IViewProjectDateInfo.LoaderCultureInfo
        Get
            Return ViewStateOrDefault("LoaderCultureInfo", Resource.CultureInfo)
        End Get
        Set(value As System.Globalization.CultureInfo)
            ViewState("LoaderCultureInfo") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public ReadOnly Property ProjectStartDate As DateTime?
        Get
            Return PRstartDate
        End Get
    End Property
    Public ReadOnly Property ProjectDeadline As DateTime?
        Get
            Return PRdeadline
        End Get
    End Property
    Public ReadOnly Property ProjectEndDate As DateTime?
        Get
            Return PRendDate
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBprojectStartDate_t)
            .setLabel(LBprojectEndDate_t)
            .setLabel(LBprojectDeadline_t)
            .setLabel(LBprojectType_t)
        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeControl(project As dtoProject, culture As System.Globalization.CultureInfo, currentShortDatePattern As String, allowEdit As Boolean) Implements IViewProjectDateInfo.InitializeControl
        CurrentPresenter.InitView(project, culture, currentShortDatePattern, allowEdit)
    End Sub
    Public Sub InitializeControl(idProject As Long) Implements IViewProjectDateInfo.InitializeControl
        CurrentPresenter.InitView(idProject)
    End Sub
    Public Sub InitializeControl(idProject As Long, allowEdit As Boolean) Implements IViewProjectDateInfo.InitializeControl
        CurrentPresenter.InitView(idProject, allowEdit)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        MLVproject.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        MLVproject.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayUnknownProject() Implements IViewProjectDateInfo.DisplayUnknownProject
        MLVproject.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadProjectInfo(project As dtoProject, setCulture As Boolean) Implements IViewProjectDateInfo.LoadProjectInfo
        MLVproject.SetActiveView(VIWdefault)
        If setCulture Then
            LoaderCultureInfo = Resource.CultureInfo
            CurrentShortDatePattern = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
        End If
        CTRLstartDateInput.ReadOnlyInput = Not AllowEdit
        CTRLdeadlineInput.ReadOnlyInput = Not AllowEdit
        If project.StartDate.HasValue Then
            PRstartDate = project.StartDate.Value
            CTRLstartDateInput.AutoInitialize(project.StartDate.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLstartDateInput.AutoInitialize("")
            PRstartDate = Nothing
        End If
        If project.EndDate.HasValue Then
            PRendDate = project.EndDate.Value
            CTRLendDateInput.AutoInitialize(project.EndDate.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLendDateInput.AutoInitialize("")
            PRendDate = Nothing
        End If
        If project.Deadline.HasValue AndAlso project.EndDate.HasValue AndAlso project.EndDate.Value > project.Deadline.Value Then
            CTRLdeadlineInput.ContainerCssClass &= " " & FieldStatus.error.ToString()
        End If
        If project.Deadline.HasValue Then
            PRdeadline = project.Deadline.Value.Date
            CTRLdeadlineInput.AutoInitialize(project.Deadline.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLdeadlineInput.AutoInitialize("")
            PRdeadline = Nothing
        End If
        If project.isPortal Then
            LBprojectType.Text = Resource.getValue("projectType.portal." & project.isPersonal)
        Else
            LBprojectType.Text = Resource.getValue("projectType.community." & project.isPersonal)
        End If
    End Sub
#End Region

    Public Sub UpdateDate(startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?))
       If startDate.Current.HasValue Then
            PRstartDate = startDate.Current.Value
            CTRLstartDateInput.AutoInitialize(startDate.Current.Value.Date.ToString(CurrentShortDatePattern))
        End If
        If endDate.Current.HasValue Then
            PRendDate = endDate.Current.Value.Date
            CTRLendDateInput.AutoInitialize(endDate.Current.Value.Date.ToString(CurrentShortDatePattern))
        End If
        If deadLine.Current.HasValue AndAlso endDate.Current.HasValue AndAlso deadLine.Current.Value > endDate.Current.Value Then
            CTRLdeadlineInput.ContainerCssClass &= " " & FieldStatus.error.ToString()
        End If

        'CTRLdeadlineInput.ValidationEnabled = AllowSave
        'If AllowSave Then
        '    CTRLdeadlineInput.ValidationExpression= 
        'End If
        If deadLine.Current.HasValue Then
            PRdeadline = deadLine.Current.Value.Date
            CTRLdeadlineInput.AutoInitialize(deadLine.Current.Value.Date.ToString(CurrentShortDatePattern))
        Else
            CTRLdeadlineInput.AutoInitialize("")
            PRdeadline = Nothing
        End If
    End Sub
    Public Sub UpdateDateAndStatus(startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?))
        PRdeadline = deadLine.Current
        PRstartDate = startDate.Current
        PRendDate = endDate.Current



        If startDate.Current.HasValue Then
            PRstartDate = startDate.Current.Value
            CTRLstartDateInput.AutoInitialize(startDate.Current.Value.ToString(CurrentShortDatePattern))
            If startDate.Status <> FieldStatus.none Then
                CTRLstartDateInput.ContainerCssClass &= " " & startDate.Status.ToString()
            End If
        Else
            CTRLstartDateInput.AutoInitialize("")
            PRstartDate = Nothing
        End If
        If endDate.Current.HasValue Then
            PRendDate = endDate.Current.Value.Date
            CTRLendDateInput.AutoInitialize(endDate.Current.Value.ToString(CurrentShortDatePattern))

            If endDate.Status <> FieldStatus.none Then
                CTRLendDateInput.ContainerCssClass &= " " & endDate.Status.ToString()
            End If
        Else
            CTRLendDateInput.AutoInitialize("")
            PRendDate = Nothing
        End If
        If deadLine.Status <> FieldStatus.none Then
            CTRLdeadlineInput.ContainerCssClass &= " " & FieldStatus.error.ToString()
        End If
        If deadLine.Current.HasValue Then
            PRdeadline = deadLine.Current.Value.Date
            CTRLdeadlineInput.AutoInitialize(deadLine.Current.Value.ToString(CurrentShortDatePattern))
        Else
            CTRLdeadlineInput.AutoInitialize("")
            PRdeadline = Nothing
        End If
    End Sub
    Private Function GetDateFromString(ByVal inputDate As String, ByVal defaultDateTime As DateTime?) As DateTime
        Try
            Dim dItems As String() = inputDate.Split("/")
            If dItems.Count = 3 Then
                If dItems(0).Count = 1 Then
                    dItems(0) = "0" + dItems(0)
                End If
                If dItems(1).Count = 1 Then
                    dItems(1) = "0" + dItems(1)
                End If
            End If
            Return DateTime.ParseExact(String.Join("/", dItems), CurrentShortDatePattern, LoaderCultureInfo.CurrentCulture.InvariantCulture)
        Catch ex As Exception
            Return defaultDateTime
        End Try
        Return defaultDateTime
    End Function
End Class