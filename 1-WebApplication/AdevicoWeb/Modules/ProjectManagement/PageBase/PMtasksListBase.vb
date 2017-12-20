Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMtasksListBase
    Inherits BaseControl
    Implements IViewTasksListBase

#Region "Implements"
    Protected Property PageContainer As PageContainerType Implements IViewTasksListBase.PageContainer
        Get
            Return ViewStateOrDefault("PageContainer", PageContainerType.Dashboard)
        End Get
        Set(value As PageContainerType)
            ViewState("PageContainer") = value
        End Set
    End Property
    Protected Property PageContext As dtoProjectContext Implements IViewTasksListBase.PageContext
        Get
            Return ViewStateOrDefault("PageContext", New dtoProjectContext())
        End Get
        Set(ByVal value As dtoProjectContext)
            Me.ViewState("PageContext") = value
        End Set
    End Property

    Protected Property PageType As PageListType Implements IViewTasksListBase.PageType
        Get
            Return ViewStateOrDefault("PageType", PageListType.ListResource)
        End Get
        Set(value As PageListType)
            ViewState("PageType") = value
        End Set
    End Property
    Protected Property CurrentFromPage As PageListType Implements IViewTasksListBase.CurrentFromPage
        Get
            Return ViewStateOrDefault("CurrentFromPage", PageListType.ListResource)
        End Get
        Set(value As PageListType)
            ViewState("CurrentFromPage") = value
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewTasksListBase.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Protected Property IdContainerCommunity As Integer Implements IViewTasksListBase.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
        End Set
    End Property
    Public Property CurrentAscending As Boolean Implements IViewTasksListBase.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Public Property CurrentOrderBy As ProjectOrderBy Implements IViewTasksListBase.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", ProjectOrderBy.Deadline)
        End Get
        Set(value As ProjectOrderBy)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Protected ReadOnly Property PortalName As String Implements IViewTasksListBase.PortalName
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
    Protected Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewTasksListBase.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            SetPager(value)
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewTasksListBase.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Public ReadOnly Property CurrentPageIndex As Integer Implements IViewTasksListBase.CurrentPageIndex
        Get
            Return Pager.PageIndex
        End Get
    End Property
    Protected ReadOnly Property UnknownUser As String Implements IViewTasksListBase.UnknownUser
        Get
            Return Resource.getValue("UnknownUser")
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
#End Region

    Protected MustOverride Sub SetPager(value As lm.Comol.Core.DomainModel.PagerBase)
    Public MustOverride Sub InitializeControl(context As dtoProjectContext, ByVal idContainerCommunity As Integer, filter As dtoItemsFilter, containerPage As PageContainerType, fromPage As PageListType, currentPage As PageListType) Implements IViewTasksListBase.InitializeControl
    Protected MustOverride Sub DisplayPager(display As Boolean) Implements IViewTasksListBase.DisplayPager
    Protected MustOverride Sub LoadedNoTasks() Implements IViewTasksListBase.LoadedNoTasks
    Protected MustOverride Function GetMyTasksCompletion() As List(Of dtoMyAssignmentCompletion) Implements IViewTasksListBase.GetMyTasksCompletion
    Public MustOverride Sub SaveMyCompletions() Implements IViewTasksListBase.SaveMyCompletions
    Protected MustOverride Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean) Implements IViewTasksListBase.DisplayTasksCompletionSaved
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleProjectManagement.ActionType) Implements IViewTasksListBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, action As ModuleProjectManagement.ActionType) Implements IViewTasksListBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Internal"
    Protected Function GetCssStatuslight(completeness As Integer)
        If completeness = 100 Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function
    Protected Function GetCssStatuslight(completeness As Integer, isCompleted As Boolean)
        If completeness = 100 AndAlso isCompleted Then
            Return "green"
        ElseIf completeness > 0 Then
            Return "yellow"
        Else
            Return "gray"
        End If
    End Function

    Protected Function GetMyCompletenessTranslation() As String
        Return Resource.getValue("GetMyCompletenessTranslation")
    End Function
    Protected Function GenerateProgressBar(items As Dictionary(Of ResourceActivityStatus, Long)) As lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar
        Dim bar As lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar = Nothing
        If Not IsNothing(items) AndAlso items.ContainsKey(ResourceActivityStatus.all) Then
            bar = New lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar(items(ResourceActivityStatus.all))

            For Each item As KeyValuePair(Of ResourceActivityStatus, Long) In items.Where(Function(i) i.Value > 0).Select(Function(i) i).ToList()
                bar.Items.Add(New lm.Comol.Core.BaseModules.WebControls.Generic.ProgressBarItem(item.Value, items(ResourceActivityStatus.all)) _
                              With {.CssClass = item.Key.ToString, .PercentageTranslation = Resource.getValue("ProgressBarItem.PercentageTranslation." & item.Key.ToString), .ValueTranslation = Resource.getValue("ProgressBarItem.ValueTranslation")})


            Next
            If bar.Items.Count = 1 Then
                bar.Items(0).DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            ElseIf bar.Items.Count > 1 Then
                bar.Items.First().DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                bar.Items.Last().DisplayOrder = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            End If
        End If
        Return bar
    End Function
#End Region




End Class