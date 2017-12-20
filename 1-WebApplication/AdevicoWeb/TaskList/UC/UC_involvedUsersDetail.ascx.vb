Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.Comunita

Public Class UC_involvedUsersDetail
    Inherits BaseControlSession
    Implements IViewUC_involvedUsersDetail

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of ModuleTaskList))
    Private _presenter As InvolvedUsersDetailUCPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property


    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList)) 'Implements iViewProjectDetailWithUsersResume.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_TaskList.Codex) _
                                          Select New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = sb.CommunityID, .Permissions = New ModuleTaskList(New Services_TaskList(sb.PermissionString))}).ToList
                If _CommunitiesPermission Is Nothing Then
                    _CommunitiesPermission = New List(Of ModuleCommunityPermission(Of ModuleTaskList))
                End If
                _CommunitiesPermission.Add(New ModuleCommunityPermission(Of ModuleTaskList)() With {.ID = 0, .Permissions = ModuleTaskList.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)})
            End If
            Return _CommunitiesPermission
        End Get
    End Property

    Public ReadOnly Property CurrentPresenter() As InvolvedUsersDetailUCPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New InvolvedUsersDetailUCPresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()
       
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_involvedUsersDetail", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With MyBase.Resource

        End With

    End Sub

    Public Property CurrentTaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_involvedUsersDetail.CurrentTaskID
        Get
            Return Me.ViewState("CurrentTaskID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentTaskID") = value
        End Set
    End Property

    Public ReadOnly Property ViewModeType() As ViewModeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("ViewToLoad"), ViewModeType.TodayTasks)
        End Get
    End Property

    Public Sub InitView(ByVal oList As List(Of dtoUsersOnQuickSelection)) Implements IViewUC_involvedUsersDetail.InitView
        Me.RPTinvolvedUsers.DataSource = oList
        Me.RPTinvolvedUsers.DataBind()
    End Sub

    Public Sub RPTinvolvedUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTinvolvedUsers.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoUsersOnQuickSelection = e.Item.DataItem
            If Not IsNothing(e.Item.DataItem) Then
                Dim oLiteral As Literal


                oLiteral = e.Item.FindControl("LTusername")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = oDto.Name
                End If

                oLiteral = e.Item.FindControl("LTroles")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = oDto.Rolelist
                End If

                oLiteral = e.Item.FindControl("LTcompleteness")
                If Not IsNothing(oLiteral) Then
                    If oDto.Completeness <> -1 Then
                        oLiteral.Text = oDto.Completeness
                    Else
                        oLiteral.Text = "&nbsp"
                    End If
                End If

            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLBuserName As Label = e.Item.FindControl("LBuserName")
            Dim oLBroles As Label = e.Item.FindControl("LBroles")
            Dim oLBcompleteness As Label = e.Item.FindControl("LBcompleteness")

            Dim oLTheaderProject As Literal = e.Item.FindControl("LTheaderProject")
            Dim oLTheaderDeadline As Literal = e.Item.FindControl("LTheaderDeadline")
            Dim oLTheaderCompleteness As Literal = e.Item.FindControl("LTheaderCompleteness")

            Try
                Me.Resource.setLabel(oLBuserName)
                Me.Resource.setLabel(oLBroles)
                Me.Resource.setLabel(oLBcompleteness)
                
            Catch ex As Exception

            End Try
        End If

    End Sub


    Public Sub RPTinvolvedUsers_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTinvolvedUsers.ItemCommand
        'Me.RPTinvolvedUsers.DataSource= 
    End Sub

End Class