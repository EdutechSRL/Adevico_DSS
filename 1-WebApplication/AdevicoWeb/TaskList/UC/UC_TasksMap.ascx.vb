Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain


Partial Public Class UC_TasksMap
    Inherits BaseControlSession
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap

    Private _Presenter As lm.Comol.Modules.Base.Presentation.TaskList.TasksMapUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public Event LoadMainPage()
    Public Event GetCanAddSubTask(ByVal CanAddSubTask As Boolean)


    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.TaskList.TasksMapUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.TaskList.TasksMapUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "Iview Property"

    Public Property ViewOnlyActiveTask() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.ViewOnlyActiveTask
        Get
            Return Me.ViewState("ViewOnlyActiveTask")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ViewOnlyActiveTask") = value
        End Set
    End Property


    Public Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property


    Public Property StartLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.StartLevel
        Get
            Return Me.ViewState("StartLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("StartLevel") = value
        End Set
    End Property

    Public Property LastLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.LastLevel
        Get
            Return Me.ViewState("LastLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("LastLevel") = value
        End Set
    End Property

    Public Property CanManage() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.CanManage
        Get
            Return Me.ViewState("CanManage")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanManage") = value
        End Set
    End Property

    Public Property CanAddSubTask() As Boolean Implements IViewUC_TasksMap.CanAddSubTask
        Get
            If IsNothing(Me.ViewState("CanAddSubTask")) Then
                Return False
            Else
                Return Me.ViewState("CanAddSubTask")
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanAddSubTask") = value
        End Set
    End Property


#End Region

#Region "Iview Sub e Function "


    Public Sub SetButton()
        Me.LThideDeletedTask.Visible = Me.CanManage

    End Sub

    Public Sub LoadTasks(ByVal ListOfTask As List(Of dtoTaskMap)) Implements IViewUC_TasksMap.LoadTasks
        'Me.ViewToLoadUC = ViewToLoad
        Me.LThideDeletedTask.Visible = Me.CanManage And (ListOfTask.Count > 1)
        Me.DIVwbsLevel.Visible = ListOfTask.Count > 1
        Me.RPlistProjectTask.DataSource = ListOfTask
        Me.RPlistProjectTask.DataBind()
        RaiseEvent GetCanAddSubTask(Me.CanAddSubTask)
    End Sub

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString)
    End Sub
#End Region

#Region "Base"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property ViewToLoadUC() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements IViewUC_TasksMap.ViewToLoadUC

        Get
            Return Me.ViewState("ViewToLoadUC")
            'lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("MainPage"), ViewModeType.TodayTasks)
        End Get

        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.ViewModeType)
            Me.ViewState("ViewToLoadUC") = value
            'ViewToLoadUC = value
        End Set
    End Property

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_GeneralMap", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBwbsLevel)
            LThideStartDate.Text = .getValue("LThideStartDate.Text")
            LThideEndDate.Text = .getValue("LThideEndDate.Text")
            LThideDeletedTask.Text = .getValue("LThideDeletedTask.Text")
            .setLabel(LBlegendaVD)
        End With
    End Sub
#End Region

    Private Sub SetStatusImage(ByVal Status As TaskStatus, ByRef IMstatus As System.Web.UI.WebControls.Image)

        Select Case Status
            Case TaskStatus.completed
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/completed20.png"
            Case TaskStatus.notStarted
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/NOTSTARTEDoe.png"
            Case TaskStatus.suspended
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/SUSPENDEDoe.png"
            Case TaskStatus.started
                IMstatus.ImageUrl = Me.BaseUrl & "images/TaskList/STARTEDoe.png"
        End Select
        IMstatus.ToolTip = Me.Resource.getValue("TaskStatus." & Status.ToString)
    End Sub

    Private Sub SetPriorityImage(ByVal Priority As TaskPriority, ByRef IMpriority As System.Web.UI.WebControls.Image)
        Select Case Priority
            Case TaskPriority.high
                IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_high.png"
            Case TaskPriority.normal
                IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_normal.png"
            Case TaskPriority.low
                IMpriority.ImageUrl = Me.BaseUrl & "images/TaskList/priority_low.png"
        End Select
        IMpriority.ToolTip = Me.Resource.getValue("TaskPriority." & Priority.ToString)
    End Sub

    Private Sub RPlistProjectTask_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles RPlistProjectTask.Init

    End Sub

    Private Sub RPlistProjectTask_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPlistProjectTask.ItemCommand
        Try
            Select Case e.CommandName
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDelete(e.CommandArgument)

                Case "undelete"
                    Me.CurrentPresenter.Undelete(e.CommandArgument)

                Case "delete"
                    Me.CurrentPresenter.Delete(e.CommandArgument)

            End Select
        Catch ex As Exception

        End Try
    End Sub


    Private Sub RPlistProjectTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPlistProjectTask.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDtoTask As dtoTaskMap
            Try
                oDtoTask = DirectCast(e.Item.DataItem, dtoTaskMap)
                If Me.ViewToLoadUC = ViewModeType.TaskAdmin Then
                    oDtoTask.Permission = Me.CurrentPresenter.AdminPermissions
                End If
                Dim oLabel As Label
                Dim oHyperLink As HyperLink
                Dim oLiteral As Literal
                Dim oLinkButton As LinkButton
                Dim oTd As HtmlTableCell
                oTd = e.Item.FindControl("TDd")
                If Not IsNothing(oTd) And Not Me.CanManage Then
                    oTd.Visible = False
                End If
                oTd = e.Item.FindControl("TDm")
                If Not IsNothing(oTd) And Not Me.CanManage Then
                    oTd.Visible = False
                End If
                If ((oDtoTask.Permission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) And Not oDtoTask.isDeleted Then
                    Me.CanAddSubTask = True
                End If
                oLinkButton = e.Item.FindControl("LNBdelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.Level = 0 Then
                        oLinkButton.Visible = oDtoTask.isDeleted And ((oDtoTask.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = oDtoTask.isDeleted And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, False, True, , True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLinkButton.ToolTip)

                    oLinkButton.CommandArgument = oDtoTask.TaskID
                End If
                oLinkButton = e.Item.FindControl("LNBvirtualDelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.Level = 0 Then
                        oLinkButton.Visible = (Not oDtoTask.isDeleted) And ((oDtoTask.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = (Not oDtoTask.isDeleted) And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, False, True, , True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/cancella.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.TaskID

                End If
                oLinkButton = e.Item.FindControl("LNBundelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.Level = 0 Then
                        oLinkButton.Visible = oDtoTask.isDeleted And ((oDtoTask.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = oDtoTask.isDeleted And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, True, True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.TaskID
                End If
                oHyperLink = e.Item.FindControl("HYPmodify")
                If Not IsNothing(oHyperLink) Then
                    oHyperLink.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    Me.Resource.setHyperLink(oHyperLink, True, True)
                    If Me.CurrentPresenter.CanManage(oDtoTask.Permission) Then
                        oHyperLink.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoTask.TaskID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TaskMap.ToString
                        oHyperLink.Visible = True
                    Else
                        oHyperLink.Visible = False
                    End If
                End If
                oLiteral = e.Item.FindControl("LTspace")
                If Not IsNothing(oLiteral) Then
                    Dim Space As String
                    Dim x As Integer = (oDtoTask.Level - Me.StartLevel)
                    For i As Integer = 1 To x
                        Space &= "&nbsp;"
                    Next
                    oLiteral.Text = Space
                End If
                oHyperLink = e.Item.FindControl("HYPtaskName")
                oLabel = e.Item.FindControl("LBtaskName")
                If (oDtoTask.Permission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                    If Not IsNothing(oHyperLink) Then
                        oHyperLink.Text = System.Web.HttpUtility.HtmlEncode(oDtoTask.TaskName)
                        oHyperLink.ToolTip = System.Web.HttpUtility.HtmlEncode(oDtoTask.TaskName)
                        If oDtoTask.Level = 0 Then
                            oHyperLink.CssClass = "ROW_ItemLink_Small_Bold"
                        End If
                        oHyperLink.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoTask.TaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&MainPage=TaskMap"
                    End If
                    If Not IsNothing(oLabel) Then
                        oLabel.Visible = False
                    End If
                Else
                    If Not IsNothing(oHyperLink) Then
                        oHyperLink.Visible = False
                    End If
                    If Not IsNothing(oLabel) Then
                        If oDtoTask.Level = 0 Then
                            oLabel.CssClass = "Titolo_campoSmall"
                        End If
                        oLabel.Text = System.Web.HttpUtility.HtmlEncode(oDtoTask.TaskName)
                        oLabel.ToolTip = System.Web.HttpUtility.HtmlEncode(oDtoTask.TaskName)
                    End If
                End If

                oLabel = e.Item.FindControl("LBwbs")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.TaskWBS
                End If
                oLabel = e.Item.FindControl("LBstartDate")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.StartDate
                End If
                oLabel = e.Item.FindControl("LBendDate")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.EndDate
                End If
                oLabel = e.Item.FindControl("LBdeadline")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.EndDate
                    If oDtoTask.EndDate < Date.Now And oDtoTask.Status <> TaskStatus.completed Then
                        oLabel.CssClass = "erroreSmall"
                    Else
                        oLabel.CssClass = "Bookmark_TestoBold"
                    End If
                End If
                oLabel = e.Item.FindControl("LBcompleteness")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.TaskCompleteness.ToString & "%"
                End If

                Dim oImage As System.Web.UI.WebControls.Image
                oImage = e.Item.FindControl("IMstatus")
                If Not IsNothing(oImage) Then
                    SetStatusImage(oDtoTask.Status, oImage)
                End If
                oImage = e.Item.FindControl("IMpriority")
                If Not IsNothing(oImage) Then
                    SetPriorityImage(oDtoTask.Priority, oImage)
                End If

                oImage = e.Item.FindControl("IMcompleteness")
                If Not IsNothing(oImage) Then
                    oImage.Height = "15"
                    oImage.Width = oDtoTask.TaskCompleteness.ToString
                    oImage.ToolTip = oDtoTask.TaskCompleteness.ToString & "%"

                    oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
                End If
            Catch ex As Exception
            End Try

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oTd As HtmlTableCell
            oTd = e.Item.FindControl("TDd")
            If Not IsNothing(oTd) And Not Me.CanManage Then
                oTd.Visible = False
            End If
            oTd = e.Item.FindControl("TDm")
            If Not IsNothing(oTd) And Not Me.CanManage Then
                oTd.Visible = False
            End If
            Dim oLabel As Label
            oLabel = e.Item.FindControl("LBdeleteTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("DeleteTitle")
            End If
            oLabel = e.Item.FindControl("LBmodifyTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("ModifyTitle")
            End If
            oLabel = e.Item.FindControl("LBtaskNameTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("TaskTitle")
            End If
            oLabel = e.Item.FindControl("LBstartDateTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("StartDateTitle")
            End If
            oLabel = e.Item.FindControl("LBendDateTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("EndDateTitle")
            End If
            oLabel = e.Item.FindControl("LBdeadlineTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("DeadlineTitle")
            End If
            oLabel = e.Item.FindControl("LBcompletenessTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("CompletenessTitle")
            End If
            oLabel = e.Item.FindControl("LBstatusTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("StatusTitle")
            End If
            oLabel = e.Item.FindControl("LBpriorityTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("PriorityTitle")
            End If
        End If
    End Sub

    Public ReadOnly Property DeletedClass(ByVal isDeleted As Boolean, ByVal isAlternating As Boolean) As String
        Get
            If isDeleted Then
                Return "deleted ROW_Disabilitate_Small"
            ElseIf isAlternating Then
                Return "ROW_Alternate_Small"
            Else
                Return "ROW_Normal_Small"
            End If
        End Get
    End Property



    Public Sub LoadDdlWBSlevel() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.LoadDdlWBSlevel
        Dim temp As String = ""
        While Me.LastLevel >= Me.StartLevel
            temp &= "<option value=""" & Me.LastLevel & """ >" & Me.LastLevel & "</option>"
            Me.LastLevel = Me.LastLevel - 1
        End While
        Me.LTddl.Text = temp
    End Sub


    Public Sub ReloadPage() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.ReloadPage

        Me.Response.Redirect(Me.Request.RawUrl)
    End Sub

    Public Sub GoToMainPage() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.GoToMainPage
        RaiseEvent LoadMainPage()
    End Sub


    Public Sub ShowErrorPopUp(ByVal ErrorName As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.ErrorType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.ShowErrorPopUp
        Dim str As String
        str = Me.Resource.getValue("Error." & ErrorName.ToString)
        Me.LTscriptPopUp.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub
End Class