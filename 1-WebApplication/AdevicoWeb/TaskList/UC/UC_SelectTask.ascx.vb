Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Modules.TaskList.DomainModel
Imports lm.Comol.Modules.TaskList.Domain

Partial Public Class UC_SelectTask
    Inherits BaseControlSession
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewSelectTask


    Private _Presenter As SelectTaskPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

#Region "BaseProperty"
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Public ReadOnly Property CurrentPresenter() As SelectTaskPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectTaskPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

#Region "IViewProperty"


    Public Property SelectedTaskID() As Long Implements IViewSelectTask.SelectedTaskID
        Get
            Return Me.ViewState("SelectedTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("SelectedTaskID") = value
        End Set
    End Property

    Public Property CurrentTaskID() As Long Implements IViewSelectTask.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property


    Public Property StartLevel() As Integer Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewSelectTask.StartLevel
        Get
            Return Me.ViewState("StartLevel")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("StartLevel") = value
        End Set
    End Property

    Public ReadOnly Property SelectStyle(ByVal IsAlternatingItem As Boolean, ByVal TaskID As Long) As String
        Get
            If TaskID = Me.SelectedTaskID Then
                Return "ROW_Selected"
            ElseIf IsAlternatingItem Then
                Return "ROW_Alternate_Small"
            Else
                Return "ROW_Normal_Small"
            End If
        End Get
    End Property

#End Region

#Region "IView Sub and Function"
    Public Sub LoadTask(ByVal ListOfTasks As IList(Of lm.Comol.Modules.TaskList.Domain.dtoSelectTask)) Implements IViewSelectTask.LoadTask
        Me.RPlistOfTask.DataSource() = ListOfTasks
        Me.RPlistOfTask.DataBind()

    End Sub


    Public Function GetSelectedTaskID() As Long Implements IViewSelectTask.GetSelectedTaskID
        Return Me.SelectedTaskID
    End Function


#End Region

    Private Sub RPlistOfTask_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPlistOfTask.ItemCommand
        If (e.CommandName = "select") Then
            Me.CurrentPresenter.SetSelectedTask(e.CommandArgument)
        End If
    End Sub


    Private Sub RPlistOfTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPlistOfTask.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDtoTask As lm.Comol.Modules.TaskList.Domain.dtoSelectTask
            Try
                oDtoTask = DirectCast(e.Item.DataItem, lm.Comol.Modules.TaskList.Domain.dtoSelectTask)
                Dim oLabel As Label
                Dim oLiteral As Literal
                Dim oLinkButton As LinkButton


                Dim Space As String = ""
                Dim x As Integer = (oDtoTask.Level - Me.StartLevel)
                For i As Integer = 1 To x
                    Space &= "&nbsp;"
                Next
                oLinkButton = e.Item.FindControl("LBTselectTask")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.CommandArgument = oDtoTask.TaskID
                    oLinkButton.Visible = (oDtoTask.Permission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate
                    Me.Resource.setLinkButtonForName(oLinkButton, "LBTselectTask", False, True)
                End If
                oLiteral = e.Item.FindControl("LTspaceWBS")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = Space
                End If
                oLabel = e.Item.FindControl("LBtaskName")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = System.Web.HttpUtility.HtmlEncode(oDtoTask.TaskName)
                End If
                oLabel = e.Item.FindControl("LBwbs")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDtoTask.TaskWBS
                End If


            Catch ex As Exception

            End Try

        ElseIf e.Item.ItemType = ListItemType.Header Then

            Dim oLabel As Label = e.Item.FindControl("LBtaskNameTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("TaskTitle")
            End If

        End If

    End Sub





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub





#Region "Base"

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_GeneralMap", "TaskList")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

        End With
    End Sub
#End Region



End Class