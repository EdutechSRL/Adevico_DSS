
Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.TaskList.Domain


Partial Public Class UC_SwichTaskMap
    Inherits BaseControlSession
    Implements IViewUC_SwichTaskMap

    Private _Presenter As SwichTaskMapUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext



    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As SwichTaskMapUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SwichTaskMapUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_GeneralMap", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBwbsLevel)

        End With
    End Sub


    Public Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_SwichTaskMap.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property



    Public Property StartLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_SwichTaskMap.StartLevel
        Get
            Return Me.ViewState("StartLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("StartLevel") = value
        End Set
    End Property


    Public Sub LoadTasks(ByVal ListOfTasks As List(Of dtoSwichTask)) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_SwichTaskMap.LoadTasks
        RPlistOfTask.DataSource = ListOfTasks
        RPlistOfTask.DataBind()

    End Sub

    Private Sub RPlistOfTask_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPlistOfTask.ItemCommand

        Select Case e.CommandName
            Case "up"
                Me.CurrentPresenter.MoveTaskWbsPrevius(e.CommandArgument)
            Case "down"
                Me.CurrentPresenter.MoveTaskWbsNext(e.CommandArgument)
        End Select

    End Sub

    Private Sub RPlistOfTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPlistOfTask.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDtoTask As dtoSwichTask
            Try
                oDtoTask = DirectCast(e.Item.DataItem, dtoSwichTask)
                Dim oLabel As Label
                Dim oHyperLink As HyperLink
                Dim oLiteral As Literal
                Dim oLinkButton As LinkButton
                Dim oImage As System.Web.UI.WebControls.Image
                oLinkButton = e.Item.FindControl("LNBup")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.Visible = (Not oDtoTask.isFirst) 'And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    Me.Resource.setLinkButtonForName(oLinkButton, "LNBup", True, True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/DG/up_over.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.TaskID
                End If
                oImage = e.Item.FindControl("IMup")
                If Not IsNothing(oImage) Then
                    oImage.Visible = (oDtoTask.isFirst) 'And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    oImage.ImageUrl = Me.BaseUrl & "images/DG/upForum.gif"
                End If
                oLinkButton = e.Item.FindControl("LNBdown")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.Visible = (Not oDtoTask.isLast) 'And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    Me.Resource.setLinkButtonForName(oLinkButton, "LNBdown", True, True)
                    oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/DG/down_over.gif", oLinkButton.ToolTip)
                    oLinkButton.CommandArgument = oDtoTask.TaskID
                End If
                oImage = e.Item.FindControl("IMdown")
                If Not IsNothing(oImage) Then
                    oImage.Visible = (oDtoTask.isLast) 'And ((oDtoTask.Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
                    oImage.ImageUrl = Me.BaseUrl & "images/DG/downForum.gif"
                End If
                Dim Space As String = ""
                Dim x As Integer = (oDtoTask.Level - Me.StartLevel)
                For i As Integer = 1 To x
                    Space &= "&nbsp;"
                Next

                oLiteral = e.Item.FindControl("LTspaceWBS")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = Space
                End If
                oLiteral = e.Item.FindControl("LTspaceButton")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = Space
                End If

                oHyperLink = e.Item.FindControl("HYPtaskName")
                If Not IsNothing(oHyperLink) Then
                    oHyperLink.Text = oDtoTask.TaskName
                    oHyperLink.ToolTip = oDtoTask.TaskName
                    oHyperLink.NavigateUrl = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & oDtoTask.TaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString
                End If
                oLabel = e.Item.FindControl("LBwbs")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.TaskWBS
                End If


                oLabel = e.Item.FindControl("LBdeadline")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.Deadline
                    If oDtoTask.Deadline < Date.Now And oDtoTask.Status <> TaskStatus.completed Then
                        oLabel.CssClass = "erroreSmall"
                    Else
                        oLabel.CssClass = "Bookmark_TestoBold"
                    End If
                End If


            Catch ex As Exception

            End Try

        ElseIf e.Item.ItemType = ListItemType.Header Then 'Da continuare

            Dim oLabel As Label
            oLabel = e.Item.FindControl("LBtaskNameTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("TaskTitle")
            End If
            oLabel = e.Item.FindControl("LBdeadlineTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("DeadlineTitle")
            End If
        End If
    End Sub


    Public Property LastLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_SwichTaskMap.LastLevel
        Get
            Return Me.ViewState("LastLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("LastLevel") = value
        End Set
    End Property

    Public Sub LoadDdlWBSlevel() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_SwichTaskMap.LoadDdlWBSlevel
        Dim temp As String = ""
        While Me.LastLevel >= Me.StartLevel
            temp &= "<option value=""" & Me.LastLevel & """ >" & Me.LastLevel & "</option>"
            Me.LastLevel = Me.LastLevel - 1
        End While
        Me.LTddl.Text = temp
    End Sub


End Class