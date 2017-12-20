Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports System.Web.Script.Services
Imports System.Web.Services

Public Class SubActText
    Inherits PageBaseEduPath

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(ActivityId, ItemType.Activity)
            End If
            Return _PathType
        End Get
    End Property

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Private _sStatus As lm.Comol.Core.DomainModel.ModuleStatus

    Public Overrides Sub BindDati()

        Dim urlViewActivity As String = Me.BaseUrl & RootObject.ViewActivity(ActivityId, ServiceEP.GetUnitId_ByActivityId(ActivityId), ServiceEP.GetPathId_ByActivityId(ActivityId), Me.CurrentCommunityID, EpViewModeType.Manage)

        Me.HYPviewActivity.NavigateUrl = urlViewActivity
        Me.HYPerror.NavigateUrl = urlViewActivity
        If SubActId = 0 Then 'new
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.SubActivity, SubActId), InteractionType.UserWithLearningObject)
            dtoSubAct = New dtoSubActText
            With dtoSubAct
                .Status = .Status Or Status.NotLocked Or Status.EvaluableDigital
                .Weight = 1
            End With
        Else 'update
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.SubActivity, SubActId), InteractionType.UserWithLearningObject)
            dtoSubAct = ServiceEP.GetSubActText(SubActId)
        End If

        Me.SetSubActText()
        setControls_ByEpType()
    End Sub
    Private Sub setControls_ByEpType()
        If isAutoEp Then
            hideControl(DIVweight)
            hideControl(DIVmandatory)
        End If


    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return ServiceEP.CheckSubActText(SubActId, ActivityId, CurrentCommunityID, CurrentUserId, CurrentCommRoleID)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SubActText", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPviewActivity, False, True)
            .setHyperLink(Me.HYPerror, False, True)
            .setButton(Me.BTNsave, True)
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBevaluateTitle)
            .setLabel(Me.LBweightTitle)
            .setLabel(Me.LBerrorEditor)
            .setCheckBox(Me.CKBvisibility)
            .setCheckBox(Me.CKBmandatory)
            Me.RBLevaluate.Items.Item(0).Text = .getValue("RBLevaluate.0")
            Me.RBLevaluate.Items.Item(1).Text = .getValue("RBLevaluate.1")
            Me.Master.ServiceTitle = .getValue("Title")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

#End Region

#Region "Property"

    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
        End Get
    End Property

    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return Me.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
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



    Private _dtoSubAct As dtoSubActText
    Public Property dtoSubAct As dtoSubActText
        Get
            If IsNothing(_dtoSubAct) Then

                _dtoSubAct = New dtoSubActText
                If SubActId > 0 Then
                    _dtoSubAct.Id = SubActId
                End If
            End If
            Return _dtoSubAct
        End Get
        Set(ByVal value As dtoSubActText)
            _dtoSubAct = value
        End Set
    End Property


#End Region

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(CurrentCommunityID, EpViewModeType.Manage)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVeduPathList.ActiveViewIndex = 1
    End Sub

    Private _isManageable As Int16 = -10
    Protected ReadOnly Property isManageable As Boolean
        Get
            If _isManageable = -10 Then

                Dim pathId As Long = ServiceEP.GetPathId_BySubActivityId(SubActId)

                _isManageable = ServiceEP.isEditablePath(pathId, Me.PageUtility.CurrentUser.ID)
            End If
            Return _isManageable
        End Get
    End Property

    Private Sub SetSubActText()
        If isManageable Then
            Me.CTRLeditorDescription.HTML = dtoSubAct.Description
            If ServiceEP.CheckStatus(dtoSubAct.Status, Status.EvaluableAnalog) Then
                Me.RBLevaluate.Items.Item(1).Selected = True
            Else
                Me.RBLevaluate.Items.Item(0).Selected = True
            End If
            Me.DIVevalMode.Visible = ServiceEP.CheckEpType(PathType, EPType.Manual)
            Me.CKBvisibility.Checked = ServiceEP.CheckStatus(dtoSubAct.Status, Status.NotLocked)
            Me.CKBmandatory.Checked = ServiceEP.CheckStatus(dtoSubAct.Status, Status.Mandatory)

            Me.TXBweight.Text = dtoSubAct.Weight
        Else
            Me.LBerror.Text = Me.Resource.getValue("")
            MLVeduPathList.SetActiveView(VIWerror)
        End If

    End Sub


    Private Sub BTNsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsave.Click
        dtoSubAct.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)

        If ServiceEP.CheckEpType(PathType, EPType.Auto) OrElse RBLevaluate.Items.Item(0).Selected Then
            dtoSubAct.Status = Status.EvaluableDigital
        Else
            dtoSubAct.Status = Status.EvaluableAnalog
        End If

        If CKBmandatory.Checked Then
            dtoSubAct.Status = dtoSubAct.Status Or Status.Mandatory
            If ServiceEP.CheckStatus(dtoSubAct.Status, Status.NotMandatory) Then
                dtoSubAct.Status = dtoSubAct.Status - Status.NotMandatory
            End If
        Else
            dtoSubAct.Status = dtoSubAct.Status Or Status.NotMandatory
            If ServiceEP.CheckStatus(dtoSubAct.Status, Status.Mandatory) Then
                dtoSubAct.Status = dtoSubAct.Status - Status.Mandatory
            End If
        End If

        If CKBvisibility.Checked Then
            dtoSubAct.Status = dtoSubAct.Status Or Status.NotLocked
            If ServiceEP.CheckStatus(dtoSubAct.Status, Status.Locked) Then
                dtoSubAct.Status = dtoSubAct.Status - Status.Locked
            End If
        Else
            dtoSubAct.Status = dtoSubAct.Status Or Status.Locked
            If ServiceEP.CheckStatus(dtoSubAct.Status, Status.NotLocked) Then
                dtoSubAct.Status = dtoSubAct.Status - Status.NotLocked
            End If
        End If

        dtoSubAct.Weight = Me.TXBweight.Text

        If dtoSubAct.Description.Length = 0 Then

            Me.LBerrorEditor.Visible = True
            Me.SetSubActText()
            Me.MLVeduPathList.ActiveViewIndex = 0
        Else

            If IsNothing(ServiceEP.SaveOrUpdateSubActivityText(dtoSubAct, Me.ActivityId, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)) Then
                Me.ShowError(EpError.Generic)
            Else
                Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(ActivityId, ServiceEP.GetUnitId_ByActivityId(ActivityId), ServiceEP.GetPathId_ByActivityId(ActivityId), Me.CurrentCommunityID, EpViewModeType.Manage))
            End If

        End If

    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        MLVeduPathList.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class