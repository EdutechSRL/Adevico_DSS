Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.TaskList.Domain

Public Class UC_ProjectWithRolesProUsersDetail
    Inherits BaseControlSession
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail


    Private _Presenter As lm.Comol.Modules.Base.Presentation.TaskList.ProjectWithRolesProUsersDetailUCPresenter
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
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.TaskList.ProjectWithRolesProUsersDetailUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.TaskList.ProjectWithRolesProUsersDetailUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_GeneralMap", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            '.setLabel(LBwbsLevel)
            'LThideStartDate.Text = .getValue("LThideStartDate.Text")
            'LThideEndDate.Text = .getValue("LThideEndDate.Text")
            'LThideDeletedTask.Text = .getValue("LThideDeletedTask.Text")
            '.setLabel(LBlegendaVD)
        End With
    End Sub

    Public Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

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


    Public Property ViewToLoadUC() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements IViewUC_ProjectWithRolesProUsersDetail.ViewToLoadUC
        Get
            Return Me.ViewState("ViewToLoadUC")
            'Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("MainPage"), ViewModeType.TodayTasks)
        End Get

        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.ViewModeType)
            Me.ViewState("ViewToLoadUC") = value
            'ViewToLoadUC = value
        End Set
    End Property

    Public Property ViewOnlyActiveTask() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.ViewOnlyActiveTask
        Get
            Return Me.ViewState("ViewOnlyActiveTask")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ViewOnlyActiveTask") = value
        End Set
    End Property

    Public Property CanManage() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.CanManage
        Get
            Return Me.ViewState("CanManage")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanManage") = value
        End Set
    End Property

    Public Property CanAddSubTask() As Boolean Implements IViewUC_ProjectWithRolesProUsersDetail.CanAddSubTask
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

    Public Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.GoToReallocateResource
        Me.PageUtility.RedirectToUrl("/TaskList/ReallocateUsers.aspx?CurrentTaskID=" & TaskID & "&CurrentModeType=" & ReallocateType.ToString)
    End Sub

    Public Property StartLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.StartLevel
        Get
            Return Me.ViewState("StartLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("StartLevel") = value
        End Set
    End Property

    Public Property LastLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.LastLevel
        Get
            Return Me.ViewState("LastLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("LastLevel") = value
        End Set
    End Property

    Private Sub RPlistProjectTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPlistProjectTask.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDtoTask As dtoQuickUserSelection
            Try
                oDtoTask = DirectCast(e.Item.DataItem, dtoQuickUserSelection)
                If Me.ViewToLoadUC = ViewModeType.TaskAdmin Then
                    oDtoTask.dtoSwitch.Permission = Me.CurrentPresenter.AdminPermissions
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
                If ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) And Not oDtoTask.dtoSwitch.isDeleted Then
                    Me.CanAddSubTask = True
                End If
                oLinkButton = e.Item.FindControl("LNBdelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.dtoSwitch.Level = 0 Then
                        oLinkButton.Visible = oDtoTask.dtoSwitch.isDeleted And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = oDtoTask.dtoSwitch.isDeleted And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, False, True, , True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLinkButton.ToolTip)

                    oLinkButton.CommandArgument = oDtoTask.dtoSwitch.TaskID
                End If
                oLinkButton = e.Item.FindControl("LNBvirtualDelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.dtoSwitch.Level = 0 Then
                        oLinkButton.Visible = (Not oDtoTask.dtoSwitch.isDeleted) And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = (Not oDtoTask.dtoSwitch.isDeleted) And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, False, True, , True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/cancella.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.dtoSwitch.TaskID

                End If
                oLinkButton = e.Item.FindControl("LNBundelete")
                If Not IsNothing(oLinkButton) Then
                    If oDtoTask.dtoSwitch.Level = 0 Then
                        oLinkButton.Visible = oDtoTask.dtoSwitch.isDeleted And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Else
                        oLinkButton.Visible = oDtoTask.dtoSwitch.isDeleted And ((oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    End If
                    Me.Resource.setLinkButton(oLinkButton, True, True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.dtoSwitch.TaskID
                End If
                oHyperLink = e.Item.FindControl("HYPmodify")
                If Not IsNothing(oHyperLink) Then
                    oHyperLink.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
                    Me.Resource.setHyperLink(oHyperLink, True, True)
                    If Me.CurrentPresenter.CanManage(oDtoTask.dtoSwitch.Permission) Then
                        oHyperLink.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoTask.dtoSwitch.TaskID.ToString & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString & "&ViewToLoad=" & ViewModeType.TaskMap.ToString
                        oHyperLink.Visible = True
                    Else
                        oHyperLink.Visible = False
                    End If
                End If
                oLiteral = e.Item.FindControl("LTspace")
                If Not IsNothing(oLiteral) Then
                    Dim Space As String
                    Dim x As Integer = (oDtoTask.dtoSwitch.Level - Me.StartLevel)
                    For i As Integer = 1 To x
                        Space &= "&nbsp;"


                    Next
                    oLiteral.Text = Space
                End If
                oHyperLink = e.Item.FindControl("HYPtaskName")
                oLabel = e.Item.FindControl("LBtaskName")
                If (oDtoTask.dtoSwitch.Permission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                    If Not IsNothing(oHyperLink) Then
                        oHyperLink.Text = System.Web.HttpUtility.HtmlEncode(oDtoTask.dtoSwitch.TaskName)
                        oHyperLink.ToolTip = System.Web.HttpUtility.HtmlEncode(oDtoTask.dtoSwitch.TaskName)
                        If oDtoTask.dtoSwitch.Level = 0 Then
                            oHyperLink.CssClass = "ROW_ItemLink_Small_Bold"
                        End If
                        oHyperLink.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoTask.dtoSwitch.TaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString & "&MainPage=TaskMap"
                    End If
                    If Not IsNothing(oLabel) Then
                        oLabel.Visible = False
                    End If
                Else
                    If Not IsNothing(oHyperLink) Then
                        oHyperLink.Visible = False
                    End If
                    If Not IsNothing(oLabel) Then
                        If oDtoTask.dtoSwitch.Level = 0 Then
                            oLabel.CssClass = "Titolo_campoSmall"
                        End If
                        oLabel.Text = System.Web.HttpUtility.HtmlEncode(oDtoTask.dtoSwitch.TaskName)
                        oLabel.ToolTip = System.Web.HttpUtility.HtmlEncode(oDtoTask.dtoSwitch.TaskName)
                    End If
                End If

                oLabel = e.Item.FindControl("LBInvolvedUser")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.QuickUserList.ToString '"You draw a blanck" 
                End If

                oLabel = e.Item.FindControl("LBwbs")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.dtoSwitch.TaskWBS
                End If
                oLabel = e.Item.FindControl("LBstartDate")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.dtoSwitch.StartDate
                End If
                oLabel = e.Item.FindControl("LBendDate")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.dtoSwitch.EndDate
                End If
                oLabel = e.Item.FindControl("LBdeadline")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.dtoSwitch.EndDate
                    If oDtoTask.dtoSwitch.EndDate < Date.Now And oDtoTask.dtoSwitch.Status <> TaskStatus.completed Then
                        oLabel.CssClass = "erroreSmall"
                    Else
                        oLabel.CssClass = "Bookmark_TestoBold"
                    End If
                End If
                oLabel = e.Item.FindControl("LBcompleteness")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.dtoSwitch.TaskCompleteness.ToString & "%"
                End If

                Dim oImage As System.Web.UI.WebControls.Image
                oImage = e.Item.FindControl("IMstatus")
                If Not IsNothing(oImage) Then
                    SetStatusImage(oDtoTask.dtoSwitch.Status, oImage)
                End If
                'oImage = e.Item.FindControl("IMpriority")
                'If Not IsNothing(oImage) Then
                '    SetPriorityImage(oDtoTask.Priority, oImage)
                'End If

                oImage = e.Item.FindControl("IMcompleteness")
                If Not IsNothing(oImage) Then
                    oImage.Height = "15"
                    oImage.Width = oDtoTask.dtoSwitch.TaskCompleteness.ToString
                    oImage.ToolTip = oDtoTask.dtoSwitch.TaskCompleteness.ToString & "%"

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

    ''Public Sub LoadDdlWBSlevel() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.LoadDdlWBSlevel
    ''    Dim temp As String = ""
    ''    While Me.LastLevel >= Me.StartLevel
    ''        temp &= "<option value=""" & Me.LastLevel & """ >" & Me.LastLevel & "</option>"
    ''        Me.LastLevel = Me.LastLevel - 1
    ''    End While
    ''    Me.LTddl.Text = temp
    ''End Sub

    Public Sub LoadTasks(ByVal ListOfTask As List(Of dtoQuickUserSelection)) Implements IViewUC_ProjectWithRolesProUsersDetail.LoadTasks
        'Me.ViewToLoadUC = ViewToLoad
        'Me.LThideDeletedTask.Visible = Me.CanManage And (ListOfTask.Count > 1)
        'Me.DIVwbsLevel.Visible = ListOfTask.Count > 1
        Me.RPlistProjectTask.DataSource = ListOfTask
        Me.RPlistProjectTask.DataBind()
        RaiseEvent GetCanAddSubTask(Me.CanAddSubTask)
    End Sub

    Public Sub ReloadPage() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.ReloadPage

        Me.Response.Redirect(Me.Request.RawUrl)
    End Sub

    Public Sub GoToMainPage() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.GoToMainPage
        RaiseEvent LoadMainPage()
    End Sub


    'Public Sub ShowErrorPopUp(ByVal ErrorName As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_TasksMap.ErrorType) Implements IViewUC_ProjectWithRolesProUsersDetail.ShowErrorPopUp
    '    Dim str As String
    '    str = Me.Resource.getValue("Error." & ErrorName.ToString)
    '    Me.LTscriptPopUp2.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    'End Sub

    Public Sub ShowErrorPopUp(ByVal ErrorName As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.ErrorType) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ProjectWithRolesProUsersDetail.ShowErrorPopUp
        Dim str As String
        str = Me.Resource.getValue("Error." & ErrorName.ToString)
        Me.LTscriptPopUp2.Text = "<script type=""text/javascript"">alert(""" + str + """);</script>"
    End Sub

    Private Sub RPlistProjectTask_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles RPlistProjectTask.Load

    End Sub
End Class