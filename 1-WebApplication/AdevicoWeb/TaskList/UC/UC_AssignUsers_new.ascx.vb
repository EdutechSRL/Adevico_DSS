Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Modules.TaskList.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Modules.TaskList.DomainModel
Imports lm.Comol.Modules.TaskList.Domain

Partial Public Class UC_AssignUsers_new
    Inherits BaseControlSession
    Implements IViewUC_AssignUsers_new

    Private _Presenter As UC_AssignUsers_newPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext


    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As UC_AssignUsers_newPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_AssignUsers_newPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "Paginazione"

    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewUC_AssignUsers_new.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            'Me.PGgrid.Pager = value
            'Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property

    Public ReadOnly Property CurrentPageSize() As Integer Implements IViewUC_AssignUsers_new.CurrentPageSize
        Get
            Return 10
        End Get
    End Property

#End Region

#Region "PageBase"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_AssignedUser", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource

            .setHyperLink(HYPaddResource, True, False)
            ' .setHeaderGridView(Me.GDVassignedPersons, , "Name", True)
            '.setHeaderGridView(Me.GDVassignedPersons, 0, "Operation", True)
            '.setHeaderGridView(Me.GDVassignedPersons, 2, "Name", True)
            '.setHeaderGridView(Me.GDVassignedPersons, 3, "Role", True)
            '.setHeaderGridView(Me.GDVassignedPersons, 4, "Completeness", True)

        End With
    End Sub

#End Region

    Public Event ReloadAllPage()

#Region "IView"

    Public Property CurrentView() As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.viewMode Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.CurrentView
        Get
            Return Me.ViewState("CurrentView")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.viewMode)
            Me.ViewState("CurrentView") = value
        End Set
    End Property

    'Tia
    Public Property CurrentPageView() As lm.Comol.Modules.TaskList.Domain.ViewModeType Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.CurrentPageView
        Get
            Return Me.ViewState("CurrentPageView")
        End Get
        Set(ByVal value As lm.Comol.Modules.TaskList.Domain.ViewModeType)
            Me.ViewState("CurrentView") = value
        End Set
    End Property


    Public Property TaskPermission() As TaskPermissionEnum Implements IViewUC_AssignUsers_new.TaskPermission
        Get
            Return Me.ViewState("TaskPermission")
        End Get
        Set(ByVal value As TaskPermissionEnum)
            Me.ViewState("TaskPermission") = value
        End Set
    End Property

    Public Property CanDeleteManager() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.CanDeleteManager
        Get
            Return Me.ViewState("CanDeleteManager")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CanDeleteManager") = value
        End Set
    End Property

    Public Property AreThereResources() As Boolean Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_AssignUsers_new.AreThereResources
        Get
            Return Me.ViewState("AreThereResources")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AreThereResources") = value
        End Set
    End Property

    Public Property CurrentTaskID() As Long Implements IViewUC_AssignUsers_new.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public Sub InitAssignedPersons(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.dtoAssignUsers)) Implements IViewUC_AssignUsers_new.InitAssignedPersons
        Me.RPText.DataSource = oList
        Me.RPText.DataBind()
        Me.HYPaddResource.NavigateUrl = Me.BaseUrl & "TaskList/ManageTaskAssignment.aspx?CurrentTaskID=" & Me.CurrentTaskID & "&ViewToLoad=" & Me.CurrentPageView.ToString & "&ViewType=" & IViewManageTaskAssignment.viewAssignmentType.AddTaskAssignment.ToString
        Me.HYPaddResource.Visible = (Me.CurrentView = IViewUC_AssignedUser.viewMode.Manage)
    End Sub

#End Region

    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.Pager = Me.PGgrid.Pager
        Me.CurrentPresenter.InitAssignedPersons()
    End Sub

    Public Sub RPText_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPText.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoAssignUsers As dtoAssignUsers = e.Item.DataItem

            If Not IsNothing(e.Item.DataItem) Then
                Dim oLTuserName, oLTtaskCompleteness As System.Web.UI.WebControls.Literal
                Dim oIMtaskCompleteness As System.Web.UI.WebControls.Image
                Dim oTDcompleteness As HtmlTableCell
                Dim oPNLcompleteness As Panel
                oLTtaskCompleteness = e.Item.FindControl("LTtaskCompleteness")
                oLTuserName = e.Item.FindControl("LTuserName")
                oLTuserName.Text = e.Item.DataItem.UserName
                oLTtaskCompleteness.Text = e.Item.DataItem.Completeness.ToString

                oIMtaskCompleteness = e.Item.FindControl("IMtaskCompleteness")
            If Not IsNothing(oIMtaskCompleteness) And oDtoAssignUsers.Completeness <> -1 Then
                oIMtaskCompleteness.Height = "15"
                oIMtaskCompleteness.Width = oDtoAssignUsers.Completeness.ToString()
                oIMtaskCompleteness.ToolTip = oDtoAssignUsers.Completeness.ToString() & "%"
                    oIMtaskCompleteness.ImageUrl = Me.BaseUrl & "images/TaskList/completeness.png"
            End If

            Dim RPTroles As Repeater
            RPTroles = e.Item.FindControl("RPTroles")
            RPTroles.DataSource = oDtoAssignUsers.Roles
            AddHandler RPTroles.ItemDataBound, AddressOf RPTroles_ItemDataBound
                RPTroles.DataBind()

                oTDcompleteness = e.Item.FindControl("TDcompleteness")

                'oTDcompleteness.Visible = AreThereResources AndAlso oDtoAssignUsers.Completeness <> -1
                oPNLcompleteness = e.Item.FindControl("PNLcompletess")
                'oPNLcompleteness.Visible = AreThereResources AndAlso oDtoAssignUsers.Completeness <> -1

                oTDcompleteness.Visible = AreThereResources
                oPNLcompleteness.Visible = AreThereResources AndAlso oDtoAssignUsers.Completeness <> -1


        End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oTDheaderCompleteness As HtmlTableCell
            oTDheaderCompleteness = e.Item.FindControl("TDheaderCompleteness")
            oTDheaderCompleteness.Visible = AreThereResources

            Dim oLTheaderUsername As Literal = e.Item.FindControl("LTheaderUsername")
            Dim oLTheaderRoles As Literal = e.Item.FindControl("LTheaderRoles")
            Dim oLTheaderCompleteness As Literal = e.Item.FindControl("LTheaderCompleteness")

            Try
                Me.Resource.setLiteral(oLTheaderUsername)
                Me.Resource.setLiteral(oLTheaderRoles)
                Me.Resource.setLiteral(oLTheaderCompleteness)

            Catch ex As Exception
            End Try
        End If

    End Sub

    Public Sub RPTroles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoRolesPerTask As dtoRolesPerTask = e.Item.DataItem
            Dim oLNBdelete, oLNBelimina, oLNBundelete As System.Web.UI.WebControls.LinkButton
            Dim oLTinnerRole As System.Web.UI.WebControls.Literal
            If Not IsNothing(e.Item.DataItem) Then
                Select Case Me.CurrentView
                    Case IViewUC_AssignUsers_new.viewMode.Edit
                        Dim CanViewButton As Boolean
                        If oDtoRolesPerTask.Role = TaskRole.Manager Then
                            CanViewButton = Me.CanDeleteManager
                        ElseIf oDtoRolesPerTask.Role = TaskRole.ProjectOwner Then
                            CanViewButton = False
                        Else
                            CanViewButton = True
                        End If

                        oLNBdelete = e.Item.FindControl("LNBdelete")
                        If Not IsNothing(oLNBdelete) Then
                            oLNBdelete.Visible = oDtoRolesPerTask.isDeleted
                            Me.Resource.setLinkButton(oLNBdelete, False, True, , True)
                            oLNBdelete.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLNBdelete.Text = String.Format(oLNBdelete.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLNBdelete.ToolTip)
                            oLNBdelete.CommandArgument = oDtoRolesPerTask.TAid
                        End If

                        oLNBelimina = e.Item.FindControl("LNBelimina")
                        If Not IsNothing(oLNBelimina) Then
                            oLNBelimina.Visible = (Not oDtoRolesPerTask.isDeleted) And CanViewButton
                            Me.Resource.setLinkButton(oLNBelimina, False, True, , True)
                            oLNBelimina.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLNBelimina.Text = String.Format(oLNBelimina.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBelimina.ToolTip)
                            oLNBelimina.CommandArgument = oDtoRolesPerTask.TAid
                        End If

                        oLNBundelete = e.Item.FindControl("LNBundelete")
                        If Not IsNothing(oLNBundelete) Then
                            oLNBundelete.Visible = oDtoRolesPerTask.isDeleted
                            Me.Resource.setLinkButton(oLNBundelete, True, True)
                            oLNBundelete.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLNBundelete.ToolTip)
                            oLNBundelete.CommandArgument = oDtoRolesPerTask.TAid
                        End If

                    Case IViewUC_AssignUsers_new.viewMode.Read
                        oLNBdelete = e.Item.FindControl("LNBdelete")
                        oLNBundelete = e.Item.FindControl("LNBundelete")
                        oLNBelimina = e.Item.FindControl("LNBelimina")

                        oLNBelimina.Visible = False
                        oLNBundelete.Visible = False
                        oLNBdelete.Visible = False
                End Select


                oLTinnerRole = e.Item.FindControl("LTinnerRole")
                If Not IsNothing(oLTinnerRole) Then
                    oLTinnerRole.Text = e.Item.DataItem.Role.ToString()
                End If
            End If
        End If
    End Sub

    Public Sub RPTroles_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) 'Handles RPTroles.ItemCommand
        Try
            Select Case e.CommandName
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDeleteTaskAssignment(e.CommandArgument)
                    RaiseEvent ReloadAllPage()
                Case "undelete"
                    Me.CurrentPresenter.UnDeleteTaskAssignment(e.CommandArgument)
                    RaiseEvent ReloadAllPage()
                Case "delete"
                    Me.CurrentPresenter.DeletePermanentlyTaskAssignment(e.CommandArgument)
                    RaiseEvent ReloadAllPage()
            End Select
        Catch ex As Exception

        End Try
    End Sub
End Class