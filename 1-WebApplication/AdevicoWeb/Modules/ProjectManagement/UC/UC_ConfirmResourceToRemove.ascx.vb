Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_ConfirmResourceToRemove
    Inherits BaseControl
    Implements IViewConfirmResourceToRemove

#Region "Context"
    Private _Presenter As ConfirmResourceToRemovePresenter
    Private ReadOnly Property CurrentPresenter() As ConfirmResourceToRemovePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ConfirmResourceToRemovePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewConfirmResourceToRemove.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property AllowDelete As Boolean Implements IViewConfirmResourceToRemove.AllowDelete
        Get
            Return ViewStateOrDefault("AllowDelete", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowDelete") = value
            BTNapplyRemoveResource.Visible = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewConfirmResourceToRemove.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewConfirmResourceToRemove.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewConfirmResourceToRemove.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Private Property IdProject As Long Implements IViewConfirmResourceToRemove.IdProject
        Get
            Return ViewStateOrDefault("IdProject", 0)
        End Get
        Set(value As Long)
            ViewState("IdProject") = value
        End Set
    End Property
    Private Property IdResource As Long Implements IViewConfirmResourceToRemove.IdResource
        Get
            Return ViewStateOrDefault("IdResource", 0)
        End Get
        Set(value As Long)
            ViewState("IdResource") = value
        End Set
    End Property
    Private Property ResourceName As String Implements IViewConfirmResourceToRemove.ResourceName
        Get
            Return ViewStateOrDefault("ResourceName", "")
        End Get
        Set(value As String)
            ViewState("ResourceName") = value
        End Set
    End Property
    Private ReadOnly Property UnknownUser As String Implements IViewConfirmResourceToRemove.UnknownUser
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property

#End Region

#End Region

#Region "Property"
    Public Property PreSelectedAction As RemoveAction
        Get
            Return ViewStateOrDefault("RemoveAction", RemoveAction.None)
        End Get
        Set(value As RemoveAction)
            ViewState("RemoveAction") = value
        End Set
    End Property
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property
    Public Event CloseWindow()
    Public Event DeletingResource(idResource As Long, resourceName As String, rAction As RemoveAction)
    Public Event DeletedResource(deleted As Boolean)
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
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
            .setButton(BTNapplyRemoveResource, True)
            .setButton(BTNcancelRemoveResource, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProject As Long, idResource As Long, resourceName As String, assignedTasks As Long, completedTasks As Long, Optional description As String = "") Implements IViewConfirmResourceToRemove.InitializeControl
        Me.CurrentPresenter.InitView(idProject, idResource, resourceName, assignedTasks, completedTasks, description)
    End Sub
    Public Sub InitializeControl(idProject As Long, idResource As Long, Optional description As String = "") Implements IViewConfirmResourceToRemove.InitializeControl
        Me.CurrentPresenter.InitView(idProject, idResource, description)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewConfirmResourceToRemove.DisplaySessionTimeout
        DVdescription.Visible = True
        LBdescription.Text = Resource.getValue("IViewConfirmResourceToRemove.DisplaySessionTimeout")
    End Sub
    Private Sub DisplayUnknownResource(Optional name As String = "") Implements IViewConfirmResourceToRemove.DisplayUnknownResource
        DVdescription.Visible = True
        If String.IsNullOrEmpty(name) Then
            LBdescription.Text = Resource.getValue("IViewConfirmResourceToRemove.DisplayUnknownResource")
        Else
            LBdescription.Text = String.Format(Resource.getValue("IViewConfirmResourceToRemove.DisplayUnknownResource.Name"), name)
        End If
    End Sub
    Private Sub NoPermissionToRemoveResource(name As String) Implements IViewConfirmResourceToRemove.NoPermissionToRemoveResource
        DVdescription.Visible = True
        LBdescription.Text = String.Format(Resource.getValue("IViewConfirmResourceToRemove.NoPermissionToRemoveResource"), name)
    End Sub

    Private Sub SetDescription(description As String, assignedTasks As Long, completedTasks As Long) Implements IViewConfirmResourceToRemove.SetDescription
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LBdescription.Text = description
            LBdescriptionAssignedTasks.Visible = Not (completedTasks = 0 AndAlso assignedTasks = 0)
            If assignedTasks > 1 AndAlso completedTasks > 1 Then
                LBdescriptionAssignedTasks.Text = String.Format(Resource.getValue("LBdescriptionAssignedTasks.n.n"), assignedTasks, completedTasks)
            ElseIf assignedTasks > 1 Then
                LBdescriptionAssignedTasks.Text = String.Format(Resource.getValue("LBdescriptionAssignedTasks.n." & completedTasks.ToString), assignedTasks)
            ElseIf completedTasks > 1 Then
                LBdescriptionAssignedTasks.Text = String.Format(Resource.getValue("LBdescriptionAssignedTasks." & assignedTasks.ToString & ".n"), completedTasks)
            Else
                LBdescriptionAssignedTasks.Text = Resource.getValue("LBdescriptionAssignedTasks." & assignedTasks.ToString & "." & completedTasks.ToString)
            End If
        End If
    End Sub

    Private Sub LoadAvailableActions(actions As List(Of RemoveAction), selected As RemoveAction) Implements IViewConfirmResourceToRemove.LoadAvailableActions
        PreSelectedAction = selected
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.AllowDelete = AllowDelete AndAlso actions.Count > 0
    End Sub
    Public Function DeleteResource(action As RemoveAction) As Boolean Implements IViewConfirmResourceToRemove.DeleteResource
        If AllowDelete Then
            Return Me.CurrentPresenter.Delete(IdResource, action)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetSelectedAction() As RemoveAction Implements IViewConfirmResourceToRemove.GetSelectedAction
        Dim value As String = Request.Form("RDremoveaction")
        If Not String.IsNullOrEmpty(value) AndAlso IsNumeric(value) Then
            Return value
        Else
            Return RemoveAction.None
        End If
        Return RemoveAction.None
    End Function

#End Region

    Private Sub BTNcancelRemoveResource_Click(sender As Object, e As System.EventArgs) Handles BTNcancelRemoveResource.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
    Private Sub BTNapplyRemoveResource_Click(sender As Object, e As System.EventArgs) Handles BTNapplyRemoveResource.Click
        If RaiseCommandEvents Then
            RaiseEvent DeletingResource(IdResource, ResourceName, GetSelectedAction())
        End If
    End Sub
    Protected Function SetChecked(action As RemoveAction) As String
        Return IIf(action = PreSelectedAction, "checked=""checked""", "")
    End Function

    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        Dim action As RemoveAction = DirectCast(e.Item.DataItem, RemoveAction)
        Dim oLabel As Label = e.Item.FindControl("LBactionDescription")
        oLabel.Text = Resource.getValue("description.RemoveAction." & action.ToString)

    End Sub
End Class