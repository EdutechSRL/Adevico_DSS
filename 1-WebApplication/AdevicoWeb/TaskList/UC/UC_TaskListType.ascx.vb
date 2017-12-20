Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel

Partial Public Class UC_TaskListType
    Inherits BaseControlWithLoad


    Public Event TypeSelected(ByVal oType As IViewAddProject.viewTaskListType)


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LNBcommunity.CommandArgument = IViewAddProject.viewTaskListType.Community
        Me.LNBpersonal.CommandArgument = IViewAddProject.viewTaskListType.Personal
        Me.LNBpersonalCommunity.CommandArgument = IViewAddProject.viewTaskListType.PersonalCommunity
    End Sub


    Public Sub DefineSelectableTypes(ByVal oList As List(Of IViewAddProject.viewTaskListType))
        Me.DIVcommunity.Visible = oList.Contains(IViewAddProject.viewTaskListType.Community)
        Me.DIVpersonal.Visible = oList.Contains(IViewAddProject.viewTaskListType.Personal)
        Me.DIVpersonalCommunity.Visible = oList.Contains(IViewAddProject.viewTaskListType.PersonalCommunity)
        Me.SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Private Sub LNBcommunity_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles LNBcommunity.Command, LNBpersonal.Command, LNBpersonalCommunity.Command
        RaiseEvent TypeSelected(e.CommandArgument)
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddProject", "TaskList")
    End Sub
    Public Overrides Sub SetInternazionalizzazione() 'questo dovrebbe caricare i nomi dei tipi... ma nn ho ben capito come...
        With Me.Resource
            .setLabel(Me.LBcommunity)
            .setLabel(Me.LBcommunityDescription)
            .setLabel(Me.LBpersonal)
            .setLabel(Me.LBpersonalCommunity)
            .setLabel(Me.LBpersonalCommunityDescription)
            .setLabel(Me.LBpersonalDescription)
            .setLinkButton(Me.LNBcommunity, True, True)
            .setLinkButton(Me.LNBpersonal, True, True)
            .setLinkButton(Me.LNBpersonalCommunity, True, True)
        End With
    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

End Class