Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.UCServices

Public Class CertificationItem
    Inherits EPpageBaseEduPath

    Private ReadOnly Property IdCommunityRole As Integer
        Get
            Dim key As String = "CurrentCommRoleID_" & PathId.ToString() & "_" & CurrentUserId.ToString
            Dim idRole As Integer = ViewStateOrDefault(key, -1)
            If idRole = -1 Then
                idRole = ServiceEP.GetIdCommunityRole(CurrentUserId, ServiceEP.GetPathIdCommunity(PathId))
                ViewState(key) = idRole
            End If
            Return idRole
        End Get
    End Property

    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return PageUtility.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return PageUtility.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property

    Private _subActId As Long = -1
    Public ReadOnly Property SubActId() As Long
        Get
            Dim qs_subActId As String = Me.Request.QueryString("SId")
            If _subActId = -1 AndAlso IsNumeric(qs_subActId) Then
                _subActId = qs_subActId

            End If
            Return _subActId
        End Get
    End Property

    Private _actId As Long = -1
    Public ReadOnly Property ActivityId() As Long
        Get
            Dim qs_actId As String = Me.Request.QueryString("AId")
            If _actId = -1 AndAlso IsNumeric(qs_actId) Then
                _actId = qs_actId

            End If
            Return _actId
        End Get
    End Property

    Private _pathId As Long = -1
    Public ReadOnly Property PathId() As Long
        Get
            Dim qs_pathId As String = Me.Request.QueryString("PId")
            If _pathId = -1 AndAlso IsNumeric(qs_pathId) Then
                _pathId = qs_pathId

            End If
            Return _pathId
        End Get
    End Property

    Private _unitId As Long = -1
    Public ReadOnly Property UnitId() As Long
        Get
            Dim qs_unitId As String = Me.Request.QueryString("UId")
            If _unitId = -1 AndAlso IsNumeric(qs_unitId) Then
                _unitId = qs_unitId

            End If
            Return _unitId
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.HYPviewActivity.Visible = True
        Me.HYPviewActivity.NavigateUrl = Me.BaseUrl & RootObject.ViewActivity(ActivityId, UnitId, PathId, CurrentCommunityID, lm.Comol.Modules.EduPath.Domain.EpViewModeType.Manage, IsMoocPath, PreloadIsFromReadOnly)
        'Me.HYPlistEduPath.Visible = ViewModeType = EpViewModeType.Manage OrElse (ServiceEP.GetEduPathCountInCommunity(Me.CurrentCommunityID, True) > 1)
        'Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType, True)

    End Sub



    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        If (CTRLcertificationAction.InitializeControl(CurrentCommunityID, PathId, UnitId, ActivityId, SubActId)) Then
        Else
            ShowError(CTRLcertificationAction.LastError)
            MLVcertificationItem.SetActiveView(VIWerror)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        ShowError(EpError.NotPermission)

        MLVcertificationItem.SetActiveView(VIWerror)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim ok As Boolean = False

        ok = ServiceEP.HasPermessi_ByItem(Of SubActivity)(SubActId, CurrentCommunityID) AndAlso ServiceEP.AdminCanUpdate(SubActId, lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity, CurrentUserId, IdCommunityRole)

        Return ok


        'Return ok


    End Function

    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Certification", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(BTNsave)
            '.setHyperLink(HYPviewActivity, False, "")
        End With

        HYPviewActivity.Text = Resource.getValue("HYPviewActivity.text")
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        LBerror.Text = errorMessage
        MLVcertificationItem.SetActiveView(VIWerror)
    End Sub

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage, IsMoocPath)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotManageable
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotManageable.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.Edit, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.PathNotFind
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)

        End Select

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

    Private Sub BTNsave_Click(sender As Object, e As System.EventArgs) Handles BTNsave.Click
        Dim ret As Long = CTRLcertificationAction.SaveSettings()


        If ret > 0 Then
            'CTRLmsg.InitializeControl(New dtoMessage() With {.Text = "OK", .Type = MessageType.success})
            CTRLmsgs.Visible = True
            CTRLmsgs.InitializeControl(Me.Resource.getValue("Success.Save"), MessageType.success)
        Else
            'CTRLmsg.InitializeControl(New dtoMessage() With {.Text = "KO", .Type = MessageType.error})
            CTRLmsgs.Visible = False
            CTRLmsgs.InitializeControl(Me.Resource.getValue("Error.Data"), MessageType.error)
        End If
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        MLVcertificationItem.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class