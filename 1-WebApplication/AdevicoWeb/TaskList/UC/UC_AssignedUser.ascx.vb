Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.Modules.TaskList.DomainModel
Imports lm.Comol.Modules.TaskList.Domain


Partial Public Class UC_AssignedUser
    Inherits BaseControlSession
    Implements IViewUC_AssignedUser



    Private _Presenter As AssignedUserUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As AssignedUserUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AssignedUserUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "Paginazione"
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewUC_AssignedUser.Pager
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

    Public ReadOnly Property CurrentPageSize() As Integer Implements IViewUC_AssignedUser.CurrentPageSize
        Get
            Return 10
        End Get
    End Property

#End Region


#Region "IView Property"
    Public Property CanDeleteManager() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignedUser.CanDeleteManager
        Get
            Return Me.ViewState("CanDeleteManager")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanDeleteManager") = value
        End Set
    End Property

    Public Property TaskPermission() As TaskPermissionEnum Implements IViewUC_AssignedUser.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public Property CurrentTaskID() As Long Implements IViewUC_AssignedUser.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public Property CurrentView() As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignedUser.viewMode Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignedUser.CurrentView
        Get
            Return Me.ViewState("CurrentView")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignedUser.viewMode)
            Me.ViewState("CurrentView") = value
        End Set
    End Property
#End Region


#Region "IView Metodi"
    Public Sub InitAssignedPersons(ByVal ListOfTaskAssignment As List(Of dtoTaskAssignment)) Implements IViewUC_AssignedUser.InitAssignedPersons

        Me.GDVassignedPersons.DataSource = ListOfTaskAssignment
        Me.GDVassignedPersons.DataBind()
        Me.GDVassignedPersons.Columns(0).Visible = (Me.CurrentView = IViewUC_AssignedUser.viewMode.Manage)
        Me.GDVassignedPersons.Columns(1).Visible = (Me.CurrentView = IViewUC_AssignedUser.viewMode.SelectActiveResource) Or (Me.CurrentView = IViewUC_AssignedUser.viewMode.SelectDeletedResource)
        Me.HYPaddResource.NavigateUrl = Me.BaseUrl & "TaskList/ManageTaskAssignment.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&ViewType=" & IViewManageTaskAssignment.viewAssignmentType.AddTaskAssignment.ToString
        Me.HYPaddResource.Visible = (Me.CurrentView = IViewUC_AssignedUser.viewMode.Manage)
        Me.DIVtitleResource.Visible = Not ((Me.CurrentView = IViewUC_AssignedUser.viewMode.SelectActiveResource) Or (Me.CurrentView = IViewUC_AssignedUser.viewMode.SelectDeletedResource) Or (Me.CurrentView = IViewUC_AssignedUser.viewMode.ViewAllResource))


    End Sub

#End Region


#Region "Base"

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_AssignedUser", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBtitolo)
            .setHyperLink(HYPaddResource, True, False)
            ' .setHeaderGridView(Me.GDVassignedPersons, , "Name", True)
            .setHeaderGridView(Me.GDVassignedPersons, 0, "Operation", True)
            .setHeaderGridView(Me.GDVassignedPersons, 2, "Name", True)
            .setHeaderGridView(Me.GDVassignedPersons, 3, "Role", True)
            .setHeaderGridView(Me.GDVassignedPersons, 4, "Completeness", True)

        End With
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

    Public Event ReloadAllPage()

    Private Sub GDVassignedPersons_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GDVassignedPersons.RowCommand
        'Dim Index As Integer = Convert.ToInt32(e.CommandArgument)
        'Dim CurrentRow As GridViewRow = GDVassignedPersons.Rows(Index)
        Select Case e.CommandName
            Case "confirmDelete"
                Me.CurrentPresenter.DeletePermanentlyTaskAssignment(e.CommandArgument)
            Case "virtualdelete"
                Me.CurrentPresenter.VirtualDeleteTaskAssignment(e.CommandArgument)
                RaiseEvent ReloadAllPage()
            Case "undelete"
                Me.CurrentPresenter.UnDeleteTaskAssignment(e.CommandArgument)
                RaiseEvent ReloadAllPage()
        End Select

    End Sub

    Public Function GetSelectedPerson()
        Dim ListOfSelectedTaskAssignmentID As New List(Of Long)
        For Each oRow As GridViewRow In Me.GDVassignedPersons.Rows
            Dim oCheckBox As HtmlInputCheckBox = oRow.FindControl("CBselectUser")
            If Not IsNothing(oCheckBox) Then
                If oCheckBox.Checked Then
                    ListOfSelectedTaskAssignmentID.Add(oCheckBox.Value)
                End If
            End If
        Next
        Return ListOfSelectedTaskAssignmentID
    End Function

    Private Sub GDVassignedPersons_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVassignedPersons.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oDtoTA As dtoTaskAssignment = TryCast(e.Row.DataItem, dtoTaskAssignment)
            Dim oLinkButton As LinkButton
            If Not IsNothing(oDtoTA) Then
                Select Case Me.CurrentView
                    Case IViewUC_AssignedUser.viewMode.Manage
                        Dim CanViewButton As Boolean
                        If oDtoTA.Role = TaskRole.Manager.ToString Then
                            CanViewButton = Me.CanDeleteManager
                        ElseIf oDtoTA.Role = TaskRole.ProjectOwner.ToString Then
                            CanViewButton = False
                        Else
                            CanViewButton = True
                        End If
                        oLinkButton = e.Row.FindControl("LNBdelete")
                        If Not IsNothing(oLinkButton) Then
                            oLinkButton.Visible = oDtoTA.isDeleted
                            Me.Resource.setLinkButtonForName(oLinkButton, "LNBdelete", True, False)
                            oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLinkButton.ToolTip)
                            oLinkButton.CommandArgument = oDtoTA.ID
                        End If
                        oLinkButton = e.Row.FindControl("LNBvirtualDelete")
                        If Not IsNothing(oLinkButton) Then
                            oLinkButton.Visible = (Not oDtoTA.isDeleted) And CanViewButton
                            Me.Resource.setLinkButtonForName(oLinkButton, "LNBvirtualDelete", True, False)
                            oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/cancella.gif", oLinkButton.ToolTip)
                            oLinkButton.CommandArgument = oDtoTA.ID

                        End If
                        oLinkButton = e.Row.FindControl("LNBundelete")
                        If Not IsNothing(oLinkButton) Then
                            oLinkButton.Visible = oDtoTA.isDeleted
                            Me.Resource.setLinkButtonForName(oLinkButton, "LNBundelete", True, False)
                            oLinkButton.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLinkButton.Text = String.Format(oLinkButton.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLinkButton.ToolTip)
                            oLinkButton.CommandArgument = oDtoTA.ID
                        End If

                    Case IViewUC_AssignedUser.viewMode.SelectActiveResource
                        Dim oCheck As HtmlInputCheckBox = e.Row.FindControl("CBselectUser")
                        If Not IsNothing(oCheck) Then
                            oCheck.Value = oDtoTA.ID
                        End If
                    Case IViewUC_AssignedUser.viewMode.SelectDeletedResource
                        Dim oCheck As HtmlInputCheckBox = e.Row.FindControl("CBselectUser")
                        If Not IsNothing(oCheck) Then
                            oCheck.Value = oDtoTA.ID
                        End If
                End Select

                Dim oLiteral As Literal = e.Row.FindControl("LTassignedPerson")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = oDtoTA.AssignedUser
                End If

                oLiteral = e.Row.FindControl("LTrole")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = oDtoTA.Role
                End If
                Dim oImage As System.Web.UI.WebControls.Image
                oImage = e.Row.FindControl("IMcompleteness")
                If Not IsNothing(oImage) And oDtoTA.Completeness <> -1 Then
                    oImage.Height = "15"
                    oImage.Width = oDtoTA.Completeness.ToString()
                    oImage.ToolTip = oDtoTA.Completeness.ToString() & "%"
                    oImage.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
                End If
                oLiteral = e.Row.FindControl("LTcompleteness")
                If Not IsNothing(oLiteral) Then
                    If (oDtoTA.Role.Equals(TaskRole.Resource.ToString) Or oDtoTA.Role.Equals(TaskRole.Customized_Resource.ToString)) Then
                        oLiteral.Text = oDtoTA.Completeness & " %"
                    Else
                        oLiteral.Text = "-"
                        oImage.Visible = False
                        Dim oDIVcompleteness As HtmlContainerControl = e.Row.FindControl("DIVcompleteness")
                        oDIVcompleteness.Visible = False
                    End If
                End If

            End If
        End If
    End Sub

    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.Pager = Me.PGgrid.Pager
        Me.CurrentPresenter.InitAssignedPersons()
    End Sub

End Class