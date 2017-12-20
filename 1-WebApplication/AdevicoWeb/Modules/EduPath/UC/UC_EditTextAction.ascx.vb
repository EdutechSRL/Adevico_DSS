Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Public Class UC_EditTextAction
    Inherits BaseControl

#Region "Contex"
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Private Property IdActionCommunity As Integer
        Get
            Return ViewStateOrDefault("IdActionCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdActionCommunity") = value
        End Set
    End Property
    Private Property IdActionPath As Long
        Get
            Return ViewStateOrDefault("IdActionPath", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionPath") = value
        End Set
    End Property
    Private Property IdActionUnit As Long
        Get
            Return ViewStateOrDefault("IdActionUnit", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionUnit") = value
        End Set
    End Property
    Private Property IdActionActivity As Long
        Get
            Return ViewStateOrDefault("IdActionActivity", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionActivity") = value
        End Set
    End Property

    Private ReadOnly Property GetPathType As EPType
        Get
            Return ViewStateOrDefault("GetPathType", ServiceEP.GetEpType(IdActionActivity, ItemType.Activity))
        End Get
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return CTRLeditorDescription.EditorClientId
        End Get
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SubActText", "EduPath")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBevaluateTitle)
            .setLabel(Me.LBweightTitle)
            .setLabel(Me.LBerrorEditor)
            .setCheckBox(Me.CKBvisibility)
            .setCheckBox(Me.CKBmandatory)
            Me.RBLevaluate.Items.Item(0).Text = .getValue("RBLevaluate.0")
            Me.RBLevaluate.Items.Item(1).Text = .getValue("RBLevaluate.1")
        End With
    End Sub
#End Region

    Public Function InitializeControl(ByVal idCommunity As Integer, ByVal idPath As Long, ByVal idUnit As Long, ByVal idActivity As Long) As Boolean
        Me.SetInternazionalizzazione()
        IdActionPath = idPath
        IdActionActivity = idActivity
        IdActionUnit = idUnit

        Return InitializeControl(idCommunity)
    End Function
    Private Function InitializeControl(ByVal idCommunity As Integer) As Boolean
        If ServiceEP.CheckSubActText(0, IdActionActivity, idCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.RolesID.FirstOrDefault()) Then
            Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
            Me.PageUtility.AddAction(idCommunity, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.SubActivity, 0), InteractionType.UserWithLearningObject)
            Dim dto As New dtoSubActText
            With dto
                .Status = .Status Or Status.NotLocked Or Status.EvaluableDigital
                .Weight = 1
            End With
            Dim pathType As EPType = GetPathType
            If ServiceEP.CheckEpType(pathType, lm.Comol.Modules.EduPath.Domain.EPType.Auto) Then
                DIVweight.Visible = False
                DIVmandatory.Visible = False
            End If
            'Me.CTRLeditorDescription.HTML = ""
            TXBmultiline.Text = "--"
            If ServiceEP.CheckStatus(dto.Status, Status.EvaluableAnalog) Then
                Me.RBLevaluate.SelectedIndex = 1
            Else
                Me.RBLevaluate.SelectedIndex = 0
            End If
            Me.DIVevalMode.Visible = ServiceEP.CheckEpType(pathType, EPType.Manual)
            Me.CKBvisibility.Checked = ServiceEP.CheckStatus(dto.Status, Status.NotLocked)
            Me.CKBmandatory.Checked = ServiceEP.CheckStatus(dto.Status, Status.Mandatory)

            Me.TXBweight.Text = dto.Weight
            Me.MLVtextAction.SetActiveView(VIWdata)
            Return True
        Else
            Me.ShowError(EpError.NotPermission)
            Return False
        End If
    End Function
    Private Sub ShowError(ByVal ErrorType As EpError)
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
        Me.MLVtextAction.SetActiveView(VIWerror)
    End Sub

    Public Function SaveAction(ByVal idCommunity As Integer, idUser As Integer) As Long
        Dim dto As New dtoSubActText
        'dto.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)
        dto.Description = removeBRfromStringEnd(TXBmultiline.Text)
        If ServiceEP.CheckEpType(GetPathType, EPType.Auto) OrElse RBLevaluate.SelectedIndex = 0 Then
            dto.Status = Status.EvaluableDigital
        Else
            dto.Status = Status.EvaluableAnalog
        End If

        If CKBmandatory.Checked Then
            dto.Status = dto.Status Or Status.Mandatory
            If ServiceEP.CheckStatus(dto.Status, Status.NotMandatory) Then
                dto.Status = dto.Status - Status.NotMandatory
            End If
        Else
            dto.Status = dto.Status Or Status.NotMandatory
            If ServiceEP.CheckStatus(dto.Status, Status.Mandatory) Then
                dto.Status = dto.Status - Status.Mandatory
            End If
        End If

        If CKBvisibility.Checked Then
            dto.Status = dto.Status Or Status.NotLocked
            If ServiceEP.CheckStatus(dto.Status, Status.Locked) Then
                dto.Status = dto.Status - Status.Locked
            End If
        Else
            dto.Status = dto.Status Or Status.Locked
            If ServiceEP.CheckStatus(dto.Status, Status.NotLocked) Then
                dto.Status = dto.Status - Status.NotLocked
            End If
        End If

        If IsNumeric(Me.TXBweight.Text) Then
            dto.Weight = Me.TXBweight.Text
        Else
            Me.TXBweight.Text = 1
            dto.Weight = 1
        End If

        If (String.IsNullOrEmpty(dto.Description)) Then
            dto.Description = "--"
        End If

        If dto.Description.Length = 0 Then
            Me.LBerrorEditor.Visible = True
            Return 0
        Else
            Dim action As SubActivity = ServiceEP.SaveOrUpdateSubActivityText(dto, IdActionActivity, idCommunity, idUser, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
            If IsNothing(action) Then
                Me.ShowError(EpError.Generic)
                Return -1
            Else
                Return action.Id
            End If
        End If
    End Function

    Private Function removeBRfromStringEnd(ByRef value As String) As String
        value = value.Trim
        If value.EndsWith("<br>") Then
            Return removeBRfromStringEnd(value.Remove(value.Length - 4)) 'vengono rimossi anche piu' br consecutivi
        Else
            Return value
        End If
    End Function
End Class