Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Public Class UC_EditCertificationAction
    Inherits BaseControl

#Region "Context"
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
    Protected _serviceQS As COL_Questionario.Business.ServiceQuestionnaire
    Protected ReadOnly Property ServiceQS As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_serviceQS) Then
                _serviceQS = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _serviceQS
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

#Region " UC Property / Events"
    Public Event UpdateContainer()
    Public Event UpdateAndCloseContainer()
    Public Property IsInAjaxPanel As Boolean
        Get
            Return ViewStateOrDefault("IsInAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInAjaxPanel") = value
            Me.BTNsave.Visible = Not value Or Not HideSaveButton
        End Set
    End Property
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property

    Public Property HideSaveButton As Boolean
        Get
            Return ViewStateOrDefault("HideSaveButton", True)
        End Get
        Set(value As Boolean)
            ViewState("HideSaveButton") = value
            Me.BTNsave.Visible = Not value
        End Set
    End Property

    Dim _listlink As List(Of dtoSubActivityLink)
    Dim _subact As dtoSubActivity

    Private Property subact As dtoSubActivity
        Get
            'Return _subact
            Return ViewStateOrDefault("subact", New dtoSubActivity())

        End Get
        Set(value As dtoSubActivity)
            '_subact = value
            ViewState("subact") = value
        End Set
    End Property

    Private Property listlink As List(Of dtoSubActivityLink)
        Get
            Return ViewStateOrDefault("ListLink", New List(Of dtoSubActivityLink))
            'Return _listlink
        End Get
        Set(value As List(Of dtoSubActivityLink))
            ViewState("ListLink") = value
            '_listlink = value
        End Set
    End Property

    Private ReadOnly Property QuizLink As List(Of Int64)
        Get
            Return (From item In listlink Select item.IdObject).ToList()
        End Get
    End Property
    Public Property QuizEnabled As Boolean
        Get
            Return DIVquizDetails.Visible
        End Get
        Set(value As Boolean)
            DIVquizDetails.Visible = value
        End Set
    End Property

    Public Property MarkEnabled As Boolean
        Get
            Return ViewStateOrDefault("MarkEnabled", False)
        End Get
        Set(value As Boolean)
            ViewState("MarkEnabled") = value
            DIVminMark.Visible = value
        End Set
    End Property

    Public Property CompletionEnabled As Boolean
        Get
            Return ViewStateOrDefault("CompletionEnabled", True)
        End Get
        Set(value As Boolean)
            ViewState("CompletionEnabled") = value
            DIVminCompletion.Visible = value
        End Set
    End Property
    Dim _QUIZNAME As String = "QUIZ NAME"

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

    Private Property IdActionSubActivity As Long
        Get
            Return ViewStateOrDefault("IdActionSubActivity", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionSubActivity") = value
        End Set
    End Property

    Private ReadOnly Property GetPathType As EPType
        Get
            Return ViewStateOrDefault("GetPathType", ServiceEP.GetEpType(IdActionActivity, ItemType.Activity))
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Certification", "EduPath")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()


        With Me.Resource
            .setLabel(LBautogenerated)
            .setLabel(LBsaveCertificate)
            .setLabel(LBactiveOnMinCompletion)
            .setLabel(LBactiveOnMinMark)
            .setLabel(LBtemplate)
            .setLabel(LBcertificateNameTitle)
            .setLabel(LBactiveAfterPathEndDate)
            .setLabel(LBusePathEndDateStatistics)
            .setLabel(LBallowWithEmptyPlaceHolders)
            '.setLabel(Me.RPTquizzes.FindControl("LBLquizTitle"))
            '.setLabel(Me.RPTquizzes.FindControl("LBLmandatoryTitle"))
            '.setLabel(Me.RPTquizzes.FindControl("LBLvisibleTitle"))
            '.setLabel(Me.LBdescriptionTitle)
            '.setLabel(Me.LBevaluateTitle)
            '.setLabel(Me.LBweightTitle)
            '.setLabel(Me.LBerrorEditor)
            '.setCheckBox(Me.CKBvisibility)
            '.setCheckBox(Me.CKBmandatory)
            'Me.RBLevaluate.Items.Item(0).Text = .getValue("RBLevaluate.0")
            'Me.RBLevaluate.Items.Item(1).Text = .getValue("RBLevaluate.1")

            CHBautogenerated.Text = .getValue("yes")
            CHBsaveCertificate.Text = .getValue("yes")
            CHBactiveOnMinCompletion.Text = .getValue("yes")
            CHBactiveOnMinMark.Text = .getValue("yes")
            CBXallowWithEmptyPlaceHolders.Text = .getValue("yes")
        End With
    End Sub
#End Region

    Public Property MultiVisible As Boolean
        Get
            Return ViewStateOrDefault("multivisibleselect", False)
        End Get
        Set(value As Boolean)
            ViewState("multivisibleselect") = value
        End Set
    End Property

    Private Sub SetName(dic As Dictionary(Of Int64, String))
        For Each item As dtoSubActivityLink In listlink
            item.Name = dic(item.IdObject)
        Next
    End Sub
    Public Function SaveSettings() As Long
        Dim result As Long = 0
        Dim version As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplateVersion = CTRLtemplate.SelectedItem
        Dim sa As SubActivity = Nothing
        If Not IsNothing(version) Then
            subact.ContentType = SubActivityType.Certificate
            subact.IdCertificate = version.IdTemplate

            subact.IdCertificateVersion = version.Id
            subact.AutoGenerated = Me.CHBautogenerated.Checked
            subact.SaveCertificate = Me.CHBsaveCertificate.Checked
            subact.ActiveOnMinCompletion = Me.CHBactiveOnMinCompletion.Checked
            subact.ActiveOnMinMark = Me.CHBactiveOnMinMark.Checked
            subact.Name = Me.TXBcertificateName.Text
            subact.UsePathEndDateStatistics = CHBusePathEndDateStatistics.Checked
            subact.ActiveAfterPathEndDate = CHBactiveAfterPathEndDate.Checked
            subact.AllowWithEmptyPlaceHolders = CBXallowWithEmptyPlaceHolders.Checked
            sa = ServiceEP.SaveOrUpdateSubActivityCertificate(subact, IdActionActivity, IdActionCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID)
            If Not IsNothing(sa) Then

                Dim sVisibile As String = Request.Form("RDOvisible")

                If (String.IsNullOrEmpty(sVisibile)) Then
                    sVisibile = "-1"
                Else
                    sVisibile = sVisibile.Replace("rdo-", "")
                End If

                Dim visibleId As Long = Int64.Parse(sVisibile)

                For Each item As dtoSubActivityLink In listlink
                    item.IdSubActivity = sa.Id

                    Dim e As RepeaterItem = (From i As RepeaterItem In RPTquizzes.Items Where
                                             (i.ItemType = ListItemType.Item Or i.ItemType = ListItemType.AlternatingItem) And CType(i.FindControl("HIDid"), HiddenField).Value = item.IdObject Select i).FirstOrDefault()

                    Dim ochb As CheckBox = e.FindControl("CHBmandatory")
                    If (Not ochb Is Nothing) Then
                        item.Mandatory = ochb.Checked
                    End If

                    If Not MultiVisible Then
                        item.Visible = (visibleId = item.IdObject)
                    Else
                        ochb = e.FindControl("CHBvisible")
                        If (Not ochb Is Nothing) Then
                            item.Visible = ochb.Checked
                        End If
                    End If
                Next
                ServiceEP.SaveDtoSubactivityLinks(listlink, PageUtility.CurrentContext.UserContext.CurrentUserID)
            End If
        End If

        If Not IsNothing(sa) Then
            result = sa.Id
        End If

        RPTquizzes.DataSource = listlink
        RPTquizzes.DataBind()
        Return result
    End Function

    Public Function ItemChecked(x As Boolean, y As Long) As String
        Return IIf(x Or ViewStateOrDefault("RDOvisible", "rdo--1") = "rdo-" + y.ToString(), "checked", "")
    End Function

    Private _isManageable As Int16 = -10
    Protected ReadOnly Property isManageable As Boolean
        Get
            If _isManageable = -10 Then
                _isManageable = ServiceEP.isEditablePath(IdActionPath, Me.PageUtility.CurrentUser.ID)
            End If
            Return _isManageable
        End Get
    End Property

    Public Function InitializeControl(ByVal idCommunity As Integer, ByVal idPath As Long, ByVal idUnit As Long, ByVal idActivity As Long, idSubActivity As Long) As Boolean
        Me.SetInternazionalizzazione()
        ViewState("RDOvisible") = "rdo--1"
        Me.IdActionCommunity = idCommunity
        Me.IdActionPath = idPath
        Me.IdActionUnit = idUnit
        Me.IdActionActivity = idActivity
        Me.IdActionSubActivity = idSubActivity


        If IdActionSubActivity = 0 Then
            subact = New dtoSubActivity() With {
                .ActivityParentId = idActivity,
                .Name = Me.Resource.getValue("DefaultCertificateName"),
                .Status = Status.NotLocked Or Status.NotMandatory, .UsePathEndDateStatistics = True, .ActiveAfterPathEndDate = True
            }
        Else
            subact = New dtoSubActivity(Me.ServiceEP.GetSubActivity(IdActionSubActivity), Status.Draft)
        End If

        listlink = Me.ServiceEP.GetDtoSubactivyLinksByPathIdAndSubActIdAndTypes(idPath, IdActionSubActivity, SubActivityType.Quiz)
        SetName(ServiceQS.GetQuestionairesName(Me.QuizLink(), PageUtility.LinguaID))
        CHBautogenerated.Checked = subact.AutoGenerated
        CHBsaveCertificate.Checked = subact.SaveCertificate
        CHBactiveOnMinCompletion.Checked = subact.ActiveOnMinCompletion
        CHBactiveOnMinMark.Checked = subact.ActiveOnMinMark
        CHBactiveAfterPathEndDate.Checked = subact.ActiveAfterPathEndDate
        CHBusePathEndDateStatistics.Checked = subact.UsePathEndDateStatistics
        CBXallowWithEmptyPlaceHolders.Checked = subact.AllowWithEmptyPlaceHolders
        TXBcertificateName.Text = subact.Name

        If (Not listlink Is Nothing And listlink.Count > 0) Then
            QuizEnabled = True
            RPTquizzes.DataSource = listlink
            RPTquizzes.DataBind()
        Else
            QuizEnabled = False
        End If


        Me.CTRLtemplate.isInAjaxPanel = Me.IsInAjaxPanel

        Dim oOwner As ModuleObject
        If subact.Id = 0 Then
            Me.CTRLtemplate.AllowSelect = True
            oOwner = ModuleObject.CreateLongObject(idActivity, COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity, idCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
        Else
            ' ROBERTO QUI DEVI VERIFICARE SE L'UTENTE PUO' O MENO CAMBIARE L'attività !! in base allo stato del percorso, ecc ecc
            If (isManageable) Then
                Me.CTRLtemplate.AllowSelect = True
                oOwner = ModuleObject.CreateLongObject(subact.Id, COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, idCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
            Else
                ShowError(EpError.NotManageable)
                MLVCertificationAction.SetActiveView(VIWerror)
                Return False
            End If
        End If

        LTactiveAfterPathEndDate.Text = Resource.getValue("LTactiveAfterPathEndDate.text")
        LTusePathEndDateStatistics.Text = Resource.getValue("LTusePathEndDateStatistics.text")


        Dim options As String = ""

        Dim path As Path = ServiceEP.GetPath(idPath)

        If (Not path.EndDate Is Nothing) Then

            If (path.FloatingDeadlines) Then
                options = Resource.getValue("EndDate.Options.Parcipipant")
            Else
                options = Resource.getValue("EndDate.Options.Date").Replace("{date}", "<span class='expireddate'>" + path.EndDate.Value.ToShortDateString() + "</span>")
            End If

        Else
            options = Resource.getValue("EndDate.Options.Not")
        End If

        LTactiveAfterPathEndDate.Text = LTactiveAfterPathEndDate.Text.Replace("{options}", "<span class='options'>" + options + "</span>")
        LTusePathEndDateStatistics.Text = LTusePathEndDateStatistics.Text.Replace("{options}", "<span class='options'>" + options + "</span>")




        Me.CTRLtemplate.InitializeControl(subact.IdCertificate, subact.IdCertificateVersion, ServiceEP.ServiceModuleID(), oOwner)
        isInitialized = True

        Return True
    End Function

    Public Function InitializeControl(ByVal idCommunity As Integer, ByVal idPath As Long, ByVal idUnit As Long, ByVal idActivity As Long) As Boolean
        Me.SetInternazionalizzazione()

        Return InitializeControl(idCommunity, idPath, idUnit, idActivity, 0)
    End Function

    Private Function InitializeControl(ByVal idCommunity As Integer) As Boolean

    End Function

    Private Sub ShowError(ByVal ErrorType As EpError)
        _error = ErrorType
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
        Me.MLVCertificationAction.SetActiveView(VIWerror)
    End Sub

    Dim _error As EpError
    Public ReadOnly Property LastError As EpError
        Get
            Return _error
        End Get
    End Property

    'Public Function SaveAction(ByVal idCommunity As Integer, idUser As Integer) As Long
    '    Dim dto As New dtoSubActText
    '    'dto.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)
    '    dto.Description = removeBRfromStringEnd(TXBmultiline.Text)
    '    If ServiceEP.CheckEpType(GetPathType, EPType.Auto) OrElse RBLevaluate.SelectedIndex = 0 Then
    '        dto.Status = Status.EvaluableDigital
    '    Else
    '        dto.Status = Status.EvaluableAnalog
    '    End If

    '    If CKBmandatory.Checked Then
    '        dto.Status = dto.Status Or Status.Mandatory
    '        If ServiceEP.CheckStatus(dto.Status, Status.NotMandatory) Then
    '            dto.Status = dto.Status - Status.NotMandatory
    '        End If
    '    Else
    '        dto.Status = dto.Status Or Status.NotMandatory
    '        If ServiceEP.CheckStatus(dto.Status, Status.Mandatory) Then
    '            dto.Status = dto.Status - Status.Mandatory
    '        End If
    '    End If

    '    If CKBvisibility.Checked Then
    '        dto.Status = dto.Status Or Status.NotLocked
    '        If ServiceEP.CheckStatus(dto.Status, Status.Locked) Then
    '            dto.Status = dto.Status - Status.Locked
    '        End If
    '    Else
    '        dto.Status = dto.Status Or Status.Locked
    '        If ServiceEP.CheckStatus(dto.Status, Status.NotLocked) Then
    '            dto.Status = dto.Status - Status.NotLocked
    '        End If
    '    End If

    '    If IsNumeric(Me.TXBweight.Text) Then
    '        dto.Weight = Me.TXBweight.Text
    '    Else
    '        Me.TXBweight.Text = 1
    '        dto.Weight = 1
    '    End If


    '    If dto.Description.Length = 0 Then
    '        Me.LBerrorEditor.Visible = True
    '        Return 0
    '    Else
    '        Dim action As SubActivity = ServiceEP.SaveOrUpdateSubActivityText(dto, IdActionActivity, idCommunity, idUser, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
    '        If IsNothing(action) Then
    '            Me.ShowError(EpError.Generic)
    '            Return -1
    '        Else
    '            Return action.Id
    '        End If
    '    End If
    'End Function

    Private Sub RPTquizzes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTquizzes.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As dtoSubActivityLink = CType(e.Item.DataItem, dtoSubActivityLink)

            If (Not item Is Nothing) Then
                Dim ohid As HiddenField = e.Item.FindControl("HIDid")
                ohid.Value = item.IdObject

                Dim olbl As Label = e.Item.FindControl("LBquiz")
                If (Not olbl Is Nothing) Then
                    olbl.Text = item.Name
                End If

                Dim ochb As CheckBox = e.Item.FindControl("CHBmandatory")
                If (Not ochb Is Nothing) Then
                    ochb.Checked = item.Mandatory
                End If

                ochb = e.Item.FindControl("CHBvisible")
                If (Not ochb Is Nothing) Then
                    ochb.Checked = item.Visible
                End If

            End If
        ElseIf (e.Item.ItemType = ListItemType.Header) Then

            Resource.setLabel(e.Item.FindControl("LBquizTitle"))
            Resource.setLabel(e.Item.FindControl("LBmandatoryTitle"))
            Resource.setLabel(e.Item.FindControl("LBvisibleTitle"))

        ElseIf (e.Item.ItemType = ListItemType.Footer) Then
            Resource.setLabel(e.Item.FindControl("LBanyone"))
        End If
    End Sub

    Private Sub BTNsave_Click(sender As Object, e As System.EventArgs) Handles BTNsave.Click
        SaveSettings()
    End Sub

    Private Sub CTRLtemplate_SelectedIndexChange() Handles CTRLtemplate.SelectedIndexChange
        'SaveSettings()
        'Dim sVisibile As String = Request.Form("RDOvisible")

        'If (String.IsNullOrEmpty(sVisibile)) Then
        '    sVisibile = "-1"
        'Else
        '    sVisibile = sVisibile.Replace("rdo-", "")
        'End If

        'Dim visibleId As Long = Int64.Parse(sVisibile)

        ViewState("RDOvisible") = Request.Form("RDOvisible")


        RPTquizzes.DataSource = listlink
        RPTquizzes.DataBind()
    End Sub
End Class